namespace OnlineStore.Events
{
    // 事件处理器接口
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        // 处理给定的事件
        void Handle(TEvent @event);
    }
}