using OnlineStore.Events;

namespace OnlineStore.Domain.Events
{
    public interface IDomainEvent : IEvent
    {
        // 获取产生领域事件的事件源对象
        IEntity Source { get; }
    }
}