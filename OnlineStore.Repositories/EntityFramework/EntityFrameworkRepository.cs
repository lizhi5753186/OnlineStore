using System;
using System.Collections.Generic;
using OnlineStore.Domain;
using System.Linq;
using OnlineStore.Domain.Specifications;
using System.Linq.Expressions;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    // 定义一个抽象类，实现代码的复用，因为我不想让每个具体的仓储类都去实现一遍Add,GetAll, Remove,Update逻辑
    // 所以定义一个抽象类来实现这些公共的逻辑，具体仓储类只需要另外实现具体逻辑
    public abstract class EntityFrameworkRepository<TAggregateRoot> : IRepository<TAggregateRoot> 
        where TAggregateRoot: class, IAggregateRoot
    {
        private readonly IEntityFrameworkRepositoryContext _efContext;

        protected EntityFrameworkRepository(IRepositoryContext context)
        {
            var efContext = context as IEntityFrameworkRepositoryContext;
            if (efContext != null) 
                this._efContext = efContext;
        }

        protected IEntityFrameworkRepositoryContext EfContext 
        {
            get { return this._efContext; }
        }

        public void Add(TAggregateRoot aggregateRoot)
        {
            // 调用IEntityFrameworkRepositoryContext的RegisterNew方法将实体添加进DbContext.DbSet对象中
            _efContext.RegisterNew(aggregateRoot);
        }

        public TAggregateRoot GetByKey(Guid key)
        {
            return _efContext.DbContex.Set<TAggregateRoot>().First(a => a.Id == key);
        }

        public TAggregateRoot GetBySpecification(ISpecification<TAggregateRoot> spec)
        {
            return _efContext.DbContex.Set<TAggregateRoot>().FirstOrDefault(spec.Expression);
        }

        public TAggregateRoot GetByExpression(Expression<Func<TAggregateRoot, bool>> expression)
        {
            return _efContext.DbContex.Set<TAggregateRoot>().FirstOrDefault(expression);
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            return GetAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.UnSpecified);
        }

        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification)
        {
            return GetAll(specification, null, SortOrder.UnSpecified);
        }


        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return GetAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }

        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var query = _efContext.DbContex.Set<TAggregateRoot>().Where(specification.Expression);
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return query.SortBy(sortPredicate).ToList();
                        break;
                    case SortOrder.Descending:
                        return query.SortByDescending(sortPredicate).ToList();
                        break;
                    default:
                        break;
                }
            }

            return query.ToList();
        }

        public bool Exists(ISpecification<TAggregateRoot> specification)
        {
            var count = _efContext.DbContex.Set<TAggregateRoot>().Count(specification.IsSatisfiedBy);
            return count != 0;
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            _efContext.RegisterDeleted(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
           _efContext.RegisterModified(aggregateRoot);
        }
    }
}