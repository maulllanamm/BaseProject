using Entities;

namespace Repositories.Interface
{
    public interface IRolePermissionRepository
    {
        public Task<List<RolePermission>> GetAll();
    }
}
