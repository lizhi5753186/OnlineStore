using System;
using OnlineStore.Domain.Events;
using OnlineStore.Infrastructure;

namespace OnlineStore.Events.Handlers
{
    [HandlesAsynchronously]
    public class SendEmailHandler : IEventHandler<OrderDispatchedEvent>, IEventHandler<OrderConfirmedEvent>
    {
        // 处理确认收货事件
        public void Handle(OrderConfirmedEvent @event)
        {
            try
            {
                Utils.SendEmail(@event.UserEmailAddress,
                    "您的订单已经确认收货",
                    string.Format("您的订单 {0} 已于 {1} 确认收货,欢迎您随时关注订单状态",
                    @event.OrderId.ToString().ToUpper(), @event.ConfirmedDate));
            }
            catch (Exception ex)
            {
                // 如遇异常，直接记Log
                Utils.Log(ex);
            }
        }

        // 处理发货事件
        public void Handle(OrderDispatchedEvent @event)
        {
            try
            {
                Utils.SendEmail(@event.UserEmailAddress,
                    "您的订单已经发货",
                    string.Format("您的订单 {0} 已于 {1} 发货, 欢迎您随时关注订单状态",
                    @event.OrderId.ToString().ToUpper(), @event.DispatchedDate));
            }
            catch (Exception ex)
            {
                // 如遇异常，直接记Log
                Utils.Log(ex);
            }
        }
    }
}