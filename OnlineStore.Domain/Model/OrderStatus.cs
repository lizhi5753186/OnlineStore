
namespace OnlineStore.Domain.Model
{
    public enum OrderStatus
    {
        Created = 0, // 订单已被创建
        Paid, // 订单已付款
        Picked, // 订单已仓库拣货
        Dispatched, // 已发货
        Delivered // 已派送
    }
}
