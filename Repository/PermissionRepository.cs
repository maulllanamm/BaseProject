using Entities;
using Repositories.Base;
using Repositories.Interface;

namespace Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private readonly DataContext _context;
        public PermissionRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
