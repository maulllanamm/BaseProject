using AutoMapper;
using DTO;
using Entities;
using Repositories.Base;
using Services.Base;
using Services.Interface;

namespace Services
{
    public class PermissionService : BaseService<PermissionDTO,Permission>, IPermissionService
    {
        private readonly IBaseRepository<Permission> _baseRepo;
        private readonly IMapper _mapper;
        public PermissionService(IBaseRepository<Permission> baseRepo, IMapper mapper) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
        }
    }
}
