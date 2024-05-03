using AutoMapper;
using DTO;
using Entities;
using Repositories.Base;
using Services.Base;
using Services.Interface;

namespace Services
{
    public class RoleService : BaseService<RoleDTO, Role>, IRoleService
    {
        private readonly IBaseRepository<Role> _repository;
        private readonly IMapper _mapper;

        public RoleService(IBaseRepository<Role> repository, IMapper mapper) : base(mapper, repository)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
