using System.Collections.Generic;
using OnlineStore.Domain.Model;

namespace OnlineStore.Domain.Repositories
{
    public interface IShoppingCartItemRepository : IRepository<ShoppingCartItem>
    {
        ShoppingCartItem FindItem(ShoppingCart shoppingCart, Product product);
    }
}