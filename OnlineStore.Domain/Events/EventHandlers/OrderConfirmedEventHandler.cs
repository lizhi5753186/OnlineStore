using OnlineStore.Domain.Model;
using OnlineStore.Events.Bus;

namespace OnlineStore.Domain.Events.EventHandlers
{
    // 订单确认事件处理器
    public class OrderConfirmedEventHandler : IDomainEventHandler<OrderConfirmedEvent>
    {
        private readonly IEventBus _bus;

        public OrderConfirmedEventHandler(IEventBus bus)
        {
            _bus = bus;
        }

        #region IDomainEventHandler Member
        // 事件处理器只对事件源的状态进行更新，事件状态的持久化而是在EventBus中进行处理的。
        public void Handle(OrderConfirmedEvent @event)
        {
            // 获得事件源对象
            var order = @event.Source as Order;
            // 更新事件源对象的属性
            if (order == null) return;

            order.DeliveredDate = @event.ConfirmedDate;
            order.Status = OrderStatus.Delivered;

            // 把事件推送到EventBus中进行进一步处理
            _bus.Publish<OrderConfirmedEvent>(@event);
        }
        #endregion 
    }
}