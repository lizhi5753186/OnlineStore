using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    public class ProductCategorizationRepository : EntityFrameworkRepository<ProductCategorization>, IProductCategorizationRepository
    {
        public ProductCategorizationRepository(IRepositoryContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetProductsForCategory(Category category)
        {
            var context = EfContext.DbContex;
            if (context == null)
                throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效.");

            var query = from product in context.Products
                        from categorization in context.ProductCategorizations
                        where product.Id == categorization.ProductId &&
                              categorization.CategoryId == category.Id
                        select product;

            return query.ToList();
        }
    }
}