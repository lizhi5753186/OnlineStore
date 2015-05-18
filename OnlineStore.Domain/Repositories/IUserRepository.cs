using OnlineStore.Domain.Model;
using OnlineStore.Domain.Specifications;

namespace OnlineStore.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool CheckPassword(string userName, string password);
    }
}