using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    public class ShoppingCartRepository : EntityFrameworkRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}