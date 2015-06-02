using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Reflection;
using OnlineStore.Infrastructure;

namespace OnlineStore.Events.Bus
{
    // 基于微软MSMQ的EventBus的实现
    public class MsmqEventBus : DisposableObject, IEventBus
    {
        #region Private Fields

        private readonly Guid _id = Guid.NewGuid();
        private volatile bool _committed = true;
        private readonly bool _useInternalTransaction;
        private readonly MessageQueue _messageQueue;
        private readonly object _lockObj = new object();
        private readonly MsmqBusOptions _options = null;
        private readonly IEventAggregator _aggregator;
        private readonly MethodInfo _publishMethod;

        #endregion

        #region Ctor

        public MsmqEventBus(string path, IEventAggregator aggregator)
        {
            this._aggregator = aggregator;
           
            _publishMethod = (from m in aggregator.GetType().GetMethods()
                let parameters = m.GetParameters()
                let methodName = m.Name
                where methodName == "Handle" &&
                      parameters != null &&
                      parameters.Length == 1
                select m).First();
            this._options = new MsmqBusOptions(path);

            this._messageQueue = new MessageQueue(path,
                _options.SharedModeDenyReceive,
                _options.EnableCache, _options.QueueAccessMode) {Formatter = _options.MessageFormatter};
            this._useInternalTransaction = _options.UseInternalTransaction && _messageQueue.Transactional;
        }

        #endregion

        #region IEventBus Members

        public Guid Id
        {
            get { return _id; }
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class, IEvent
        {
            lock (_lockObj)
            {
                _messageQueue.Send(message);
                _committed = false;
            }
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : class, IEvent
        {
            lock (_lockObj)
            {
                messages.ToList().ForEach(m =>
                {
                    _messageQueue.Send(m);
                    _committed = false;
                });
            }
        }

        public void Clear()
        {
            lock (_lockObj)
            {
                this._messageQueue.Close();
            }
        }

        public void Commit()
        {
            lock (_lockObj)
            {
                if (this._useInternalTransaction)
                {
                    using (var transaction = new MessageQueueTransaction())
                    {
                        try
                        {
                            transaction.Begin();
                            var evnt = _messageQueue.Receive();
                            if (evnt != null)
                            {
                                var evntType = evnt.GetType();
                                var method = _publishMethod.MakeGenericMethod(evntType);
                                method.Invoke(_aggregator, new object[] { evnt });
                                transaction.Commit();
                            }
                        }
                        catch
                        {
                            transaction.Abort();
                            throw;
                        }
                    }
                }
                else
                {
                    var evnt = _messageQueue.Receive();
                    if (evnt != null)
                    {
                        var evntType = evnt.GetType();
                        var method = _publishMethod.MakeGenericMethod(evntType);
                        method.Invoke(_aggregator, new object[] { evnt });
                    }
                }
            }

            _committed = true;
        }

        #endregion

        #region DisposableObject Members

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_messageQueue == null) return;

            _messageQueue.Close();
            _messageQueue.Dispose();
        }

        #endregion
    }
}
