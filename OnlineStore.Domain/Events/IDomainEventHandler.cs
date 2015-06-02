using OnlineStore.Events;

namespace OnlineStore.Domain.Events
{
    /// <summary>
    /// 领域事件处理器
    /// </summary>
    public interface IDomainEventHandler<in TDomainEvent> : IEventHandler<TDomainEvent>
        where TDomainEvent :class, IDomainEvent
    {
         
    }
}