using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    // 类别仓储的实现
    public class CategoryRepository : EntityFrameworkRepository<Category>, ICategoryRepository
    {

        public CategoryRepository(IRepositoryContext context) 
            : base(context)
        {
        }

        
    }
}