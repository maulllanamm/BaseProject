using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
    }
}
