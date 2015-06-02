using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using OnlineStore.Infrastructure;

namespace OnlineStore.Events.Bus
{
    // 领域事件处理器只是对事件对象的状态进行更新
    // 后续的事件处理操作交给EventBus进行处理
    // 本案例中EventBus主要处理的任务就是发送邮件通知，
    // 在EventBus一般处理应用事件，而领域事件处理器一般处理领域事件
    public class EventBus : DisposableObject, IEventBus
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly ThreadLocal<Queue<object>> _messageQueue = new ThreadLocal<Queue<object>>(() => new Queue<object>());
        private readonly IEventAggregator _aggregator;
        private readonly ThreadLocal<bool> _committed = new ThreadLocal<bool>(() => true);
        private readonly MethodInfo _handleMethod;

        public EventBus(IEventAggregator aggregator)
        {
            this._aggregator = aggregator;

            // 获得EventAggregator中的Handle方法
            _handleMethod = (from m in aggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Handle" &&
                             parameters != null &&
                             parameters.Length == 1
                             select m).First();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _messageQueue.Dispose();
                _committed.Dispose();
            }
        }

        #region IBus Members

        public Guid Id
        {
            get { return _id; }
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent
        {
            _messageQueue.Value.Enqueue(message);
            _committed.Value = false;
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent
        {
            foreach (var message in messages)
                Publish(message);
        }

        public void Clear()
        {
            _messageQueue.Value.Clear();
            _committed.Value = true;
        }

        #endregion

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return _committed.Value; }
        }

        // 触发应用事件处理器对事件进行处理
        public void Commit()
        {
            while (_messageQueue.Value.Count > 0)
            {
                var evnt = _messageQueue.Value.Dequeue();
                var evntType = evnt.GetType();
                var method = _handleMethod.MakeGenericMethod(evntType);
                // 调用应用事件处理器来对应用事件进行处理
                method.Invoke(_aggregator, new object[] { evnt });
            }
            _committed.Value = true;
        }

        public void Rollback()
        {
            Clear();
        }

        #endregion
    }
}