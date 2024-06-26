﻿using AutoMapper;
using DTO;
using Entities;
using Repositories.Base;
using Repositories.Interface;
using Services.Base;
using Services.Interface;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : BaseGuidService<UserDTO, User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IBaseGuidRepository<User> _baseRepository;
        public UserService(IMapper mapper, IBaseGuidRepository<User> baseRepository, IUserRepository repository) : base(mapper, baseRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _baseRepository = baseRepository;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var entities = await base.GetAll();
            return _mapper.Map<List<UserDTO>>(entities);
        }

        public async Task<List<UserDTO>> GetByListId(List<Guid> listId)
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
        public async Task<UserDTO> GetById(Guid id)
        {
            var entity = await base.GetById(id);
            return _mapper.Map<UserDTO>(entity);
        }        
        
        public async Task<UserDTO> GetByUsername(string username)
        {
            var entity = await _repository.GetByUsername(username);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO> GetByVerifyToken(string verifyToken)
        {
            var entity = await _repository.GetByVerifyToken(verifyToken);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            var entity = await _repository.GetByEmail(email);
            return _mapper.Map<UserDTO>(entity);
        }        
        
        public async Task<UserDTO> GetByPasswordResetToken(string passwordToken)
        {
            var entity = await _repository.GetByPasswordResetToken(passwordToken);
            return _mapper.Map<UserDTO>(entity);
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
        public async Task<Guid> Delete(Guid id)
        {
            return await base.Delete(id);
        }
        public async Task<string> DeleteBulk(List<UserDTO> userDto)
        {
            var res = await base.DeleteBulk(userDto);
            return res;
        }

        public async Task<Guid> SoftDelete(Guid id)
        {
            return await base.SoftDelete(id);
        }
        public async Task<string> SoftDeleteBulk(List<Guid> listId)
        {
            var res = await base.SoftDeleteBulk(listId);
            return res;
        }
    }
}
