using Entities;

namespace Services.Interface
{
    public interface IRolePermissionService 
    {
        public Task<List<RolePermission>> GetAll();
    }
}
