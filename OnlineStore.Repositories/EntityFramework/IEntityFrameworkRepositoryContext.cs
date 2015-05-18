using OnlineStore.Domain.Repositories;

namespace OnlineStore.Repositories.EntityFramework
{
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        #region Properties
        OnlineStoreDbContext DbContex { get; }
        #endregion 
    }
}
