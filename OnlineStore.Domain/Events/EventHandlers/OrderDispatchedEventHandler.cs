using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Events.Bus;

namespace OnlineStore.Domain.Events.EventHandlers
{
    // 发货事件处理器
    public class OrderDispatchedEventHandler : IDomainEventHandler<OrderDispatchedEvent>
    {
        private readonly IEventBus _bus;
       

        public OrderDispatchedEventHandler(IEventBus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderDispatchedEvent @event)
        {
            // 获得事件源对象
            var order = @event.Source as Order;
            // 更新事件源对象的属性
            if (order == null) return;

            order.DispatchedDate = @event.DispatchedDate;
            order.Status = OrderStatus.Dispatched;

            // 这里把领域事件认为是一种消息，推送到EventBus中进行进一步处理。
            _bus.Publish<OrderDispatchedEvent>(@event);
        }
    }
}