using Entities;
using Repositories.Base;

namespace Repositories
{
    public class UserRepository : BaseGuidRepository<User>, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll()
        {
            return await base.GetAll();
        }
    }
}
