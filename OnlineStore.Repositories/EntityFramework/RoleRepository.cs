using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    public class RoleRepository : EntityFrameworkRepository<Role>, IRoleRepository
    {
        public RoleRepository(IRepositoryContext context)
            : base(context)
        { }

    }
}