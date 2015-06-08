using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    // 订单仓储的实现类
    public class OrderRepository : EntityFrameworkRepository<Order>, IOrderRepository
    {
        public OrderRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}