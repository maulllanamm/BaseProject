using AutoMapper;
using DTO;
using Entities;
using Repositories.Base;
using Services.Base;
using System.Diagnostics;

namespace Services
{
    public class UserService : BaseService<UserDTO, User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<User> _repository;
        public UserService(IMapper mapper, IBaseRepository<User> repository) : base(mapper, repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var entities = await base.GetAll();
            return _mapper.Map<List<UserDTO>>(entities);
        }

        public async Task<List<UserDTO>> GetByListId(List<int> listId)
        {
            var entities = await base.GetByListId(listId);
            return _mapper.Map<List<UserDTO>>(entities);
        }

        public async Task<List<UserDTO>> GetByListProperty(string field, string[] values)
        {
            var entities = await base.GetByListProperty(field, values);
            return _mapper.Map<List<UserDTO>>(entities);
        }

        public async Task<List<UserDTO>> GetAll(int page)
        {
            var entities = await base.GetAll(page);
            return _mapper.Map<List<UserDTO>>(entities);
        }
        public async Task<UserDTO> GetById(int id)
        {
            var entity = await base.GetById(id);
            return _mapper.Map<UserDTO>(entity);
        }
        public async Task<UserDTO> Create(UserDTO userDto)
        {
            var res = await base.Create(userDto);
            return _mapper.Map<UserDTO>(res);
        }
        public async Task<string> CreateBulk(List<UserDTO> userDto)
        {
            var res = await base.CreateBulk(userDto);
            return res;
        }

        public async Task<UserDTO> Update(UserDTO userDto)
        {
            var res = await base.Update(userDto);
            return _mapper.Map<UserDTO>(res);
        }

        public async Task<string> UpdateBulk(List<UserDTO> userDto)
        {
            var res = await base.UpdateBulk(userDto);
            return res;
        }
        public async Task<int> Delete(int id)
        {
            return await base.Delete(id);
        }
        public async Task<string> DeleteBulk(List<UserDTO> userDto)
        {
            var res = await base.DeleteBulk(userDto);
            return res;
        }

        public async Task<int> SoftDelete(int id)
        {
            return await base.SoftDelete(id);
        }
        public async Task<string> SoftDeleteBulk(List<UserDTO> userDto)
        {
            var res = await base.SoftDeleteBulk(userDto);
            return res;
        }
    }
}
