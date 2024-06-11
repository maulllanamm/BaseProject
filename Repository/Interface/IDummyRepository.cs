using Entities;

namespace Repositories.Interface
{
    public interface IDummyRepository
    {
        Task<Dummy> GetByUsername(string username);
        Task<Dummy> GetByEmail(string email);
    }
}
