using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Domain.Model;

namespace OnlineStore.Domain.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        #region ISpecification<T> Members
        public bool IsSatisfiedBy(T candidate)
        {
            return this.Expression.Compile()(candidate);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }
        #endregion 
    }
}
