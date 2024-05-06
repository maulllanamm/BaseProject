using Entities;
using Repositories.Base;
using Repositories.Interface;

namespace Repositories
{
    public class UserRepository : BaseGuidRepository<User>, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.username == username);
        }        

        public async Task<User> GetByVerifyToken(string verifyToken)
        {
            return _context.Users.FirstOrDefault(x => x.verify_token == verifyToken);
        }
    }
}
