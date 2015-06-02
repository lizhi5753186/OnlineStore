using OnlineStore.Domain.Model;

namespace OnlineStore.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        /// <summary>
        /// 根据指定的用户，获取该用户所属的角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Role GetRoleForUser(User user);
    }
}