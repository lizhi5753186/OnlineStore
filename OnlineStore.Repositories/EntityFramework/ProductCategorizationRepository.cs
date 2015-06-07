using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure;

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

        public PagedResult<Product> GetProductsForCategoryWithPagination(Category category, int pageNumber, int pageSize)
        {
            var context = EfContext.DbContex;
            if (context == null)
                throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效.");

            var skip = (pageNumber - 1)*pageSize;
            var take = pageSize;

            var query = from product in context.Products
                        from categorization in context.ProductCategorizations
                        where product.Id == categorization.ProductId &&
                              categorization.CategoryId == category.Id
                        orderby product.Name ascending
                        select product;

            var pagedQuery = query.Skip(skip).Take(take).GroupBy(p => new {Total = query.Count()}).FirstOrDefault();
            return pagedQuery == null ? null : new PagedResult<Product>(pagedQuery.Key.Total, (pagedQuery.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedQuery.Select(p => p).ToList());
        }
        public Category GetCategoryForProduct(Product product)
        {
            var context = EfContext.DbContex;
            if (context == null) throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效。");

            var query = from category in context.Categories
                from categorization in context.ProductCategorizations
                where categorization.ProductId == product.Id &&
                      categorization.CategoryId == category.Id
                select category;
            return query.FirstOrDefault();
        }
    }
}