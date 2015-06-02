using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    public class OrderRepository : EntityFrameworkRepository<Order>, IOrderRepository
    {
        public OrderRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}