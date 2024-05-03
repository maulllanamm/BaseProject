using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DataContext _context;
        public RolePermissionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<RolePermission>> GetAll()
        {
            return await _context.RolePermissions.ToListAsync();
        }


    }
}
