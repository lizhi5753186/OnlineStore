using System;
using System.Collections.Generic;

namespace OnlineStore.Events
{
    public interface IEventAggregator
    {
        void Register<TEvent>(IEventHandler<TEvent> domainEventHandler)
           where TEvent : class, IEvent;
        void Register<TEvent>(IEnumerable<IEventHandler<TEvent>> domainEventHandlers)
            where TEvent : class, IEvent;

        void Handle<TEvent>(TEvent domainEvent)
           where TEvent : class, IEvent;
        void Handle<TEvent>(TEvent domainEvent, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent;
    }
}