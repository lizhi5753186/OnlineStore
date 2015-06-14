using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Events;
using OnlineStore.Infrastructure;

namespace OnlineStore.Domain.Events
{
    [SuppressMessage("ReSharper", "AccessToForEachVariableInClosure")]
    public class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity _source;
        private Guid _id = Guid.NewGuid();
        private DateTime _timeStamp = DateTime.UtcNow;

        #endregion 

        #region Ctor

        public DomainEvent()
        {
        }

        public DomainEvent(IEntity source)
        {
            _source = source;
        }

        #endregion

        #region IDomainEvent Members
        public IEntity Source
        {
            get { return _source; }
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
        #endregion 

        #region Public Static Methods

        public static void Handle<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            // 找到对应的事件处理器来对事件进行处理
            var handlers = ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
            foreach (var handler in handlers)
            {
                if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                    Task.Factory.StartNew(() => handler.Handle(domainEvent));
                else
                    handler.Handle(domainEvent);
            }
        }

        public static void Handle<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            var handlers = ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
            if (handlers != null && handlers.Any())
            {
                var tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent)));
                        }
                        else
                            handler.Handle(domainEvent);
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
                callback(domainEvent, false, null);
        }
        #endregion
    }
}