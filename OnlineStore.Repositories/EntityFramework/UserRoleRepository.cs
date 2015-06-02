using System;
using System.Linq;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;


namespace OnlineStore.Repositories.EntityFramework
{
    public class UserRoleRepository :EntityFrameworkRepository<UserRole>, IUserRoleRepository
    {
         public UserRoleRepository(IRepositoryContext context)
            : base(context) { }

        public Role GetRoleForUser(User user)
        {
            var context = EfContext.DbContex as OnlineStoreDbContext;
            if (context != null)
            {
                var query = from role in context.Roles
                            from userRole in context.UserRoles
                            from usr in context.Users
                            where role.Id == userRole.RoleId &&
                                usr.Id == userRole.UserId &&
                                usr.Id == user.Id
                            select role;
                return query.FirstOrDefault();
            }
            throw new InvalidOperationException("The provided repository context is invalid.");
        }
    }
}