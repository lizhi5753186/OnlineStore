using OnlineStore.Domain.Model;
using System;
using System.Collections.Generic;

namespace OnlineStore.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetNewProducts(int count = 0);
    }
}
