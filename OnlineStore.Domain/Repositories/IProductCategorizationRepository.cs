using System.Collections.Generic;
using OnlineStore.Domain.Model;

namespace OnlineStore.Domain.Repositories
{
    public interface IProductCategorizationRepository : IRepository<ProductCategorization>
    {
        // 获取指定分类下的所有商品信息
        IEnumerable<Product> GetProductsForCategory(Category category); 
    }
}