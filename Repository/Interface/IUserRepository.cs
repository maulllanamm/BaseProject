using Entities;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
        Task<User> GetByVerifyToken(string verifyToken);
        Task<User> GetByEmail(string verifyToken);
        Task<User> GetByPasswordResetToken(string passwordToken);
    }
}
