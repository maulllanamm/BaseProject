using AutoMapper;
using Entities;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repo;
        private readonly IMapper _mapper;
        public RolePermissionService(IMapper mapper, IRolePermissionRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<List<RolePermission>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}
