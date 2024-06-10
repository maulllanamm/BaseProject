using Entities;
using Repositories.Base;
using Repositories.Interface;

namespace Repositories
{
    public class DummyRepository : BaseGuidRepository<Dummy>, IDummyRepository
    {
        private readonly DataContext _context;
        public DummyRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dummy> GetByUsername(string username)
        {
            return _context.Dummys.FirstOrDefault(x => x.username == username);
        }
        public async Task<Dummy> GetByEmail(string email)
        {
            return _context.Dummys.FirstOrDefault(x => x.email == email);
        }

      
    }
}
