using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Specifications;

namespace OnlineStore.Repositories.EntityFramework
{
    public class ShoppingCartItemRepository : EntityFrameworkRepository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(IRepositoryContext context) 
            : base(context)
        {
        }

        #region IShoppingCartItemRepository Members
        public ShoppingCartItem FindItem(ShoppingCart shoppingCart, Product product)
        {
            return GetBySpecification(Specification<ShoppingCartItem>.Eval
                (sci => sci.ShoopingCart.Id == shoppingCart.Id &&
                 sci.Product.Id == product.Id), elp => elp.Product);
        }

        #endregion
    }
}