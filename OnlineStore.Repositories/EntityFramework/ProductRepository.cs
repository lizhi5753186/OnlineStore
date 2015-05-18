using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    // 商品仓储的实现
    public class ProductRepository : EntityFrameworkRepository<Product>, IProductRepository
    {
        #region Ctor

        public ProductRepository(IRepositoryContext context)
            : base(context)
        {
        }
        #endregion 

        // 获得最新方法
        public IEnumerable<Product> GetNewProducts(int count = 0)
        {
            var ctx = this.EfContext.DbContex as OnlineStoreDbContext;
            if (ctx == null)
                return null;
            var query = from p in ctx.Products
                        where p.IsNew == true
                        select p;
            if (count == 0)
                return query.ToList();
            else
                return query.Take(count).ToList();
        }
    }
}
