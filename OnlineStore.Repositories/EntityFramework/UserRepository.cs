using System;
using System.Linq.Expressions;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Specifications;

namespace OnlineStore.Repositories.EntityFramework
{
    public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
    {
        public UserRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public bool CheckPassword(string userName, string password)
        {
            Expression<Func<User, bool>> userNameExpression = u => u.UserName == userName;
            Expression<Func<User, bool>> passwordExpression = u => u.Password == password;

            return Exists(new ExpressionSpecification<User>(userNameExpression.And(passwordExpression)));
        }
    }
}