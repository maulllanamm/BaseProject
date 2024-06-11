using AutoMapper;
using DTO;
using Entities;
using Repositories.Base;
using Repositories.Interface;
using Services.Base;
using Services.Interface;
using System.Diagnostics;

namespace Services
{
    public class DummyService : BaseGuidService<DummyDTO, Dummy>, IDummyService
    {
        private readonly IMapper _mapper;
        private readonly IDummyRepository _repository;
        private readonly IBaseGuidRepository<Dummy> _baseRepository;

        public DummyService(IMapper mapper, IDummyRepository repository, IBaseGuidRepository<Dummy> baseRepository) : base(mapper, baseRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _baseRepository = baseRepository;
        }

        public async Task<List<DummyDTO>> GetAll()
        {
            var entities = await base.GetAll();
            return _mapper.Map<List<DummyDTO>>(entities);
        }

        public async Task<List<DummyDTO>> GetByListId(List<Guid> listId)
        {
            var entities = await base.GetByListId(listId);
            return _mapper.Map<List<DummyDTO>>(entities);
        }

        public async Task<List<DummyDTO>> GetByListProperty(string field, string[] values)
        {
            var entities = await base.GetByListProperty(field, values);
            return _mapper.Map<List<DummyDTO>>(entities);
        }

        public async Task<List<DummyDTO>> GetAll(int page)
        {
            var entities = await base.GetAll(page);
            return _mapper.Map<List<DummyDTO>>(entities);
        }
        public async Task<DummyDTO> GetById(Guid id)
        {
            var entity = await base.GetById(id);
            return _mapper.Map<DummyDTO>(entity);
        }

        public async Task<DummyDTO> GetByUsername(string username)
        {
            var entity = await _repository.GetByUsername(username);
            return _mapper.Map<DummyDTO>(entity);
        }

        public async Task<DummyDTO> GetByEmail(string email)
        {
            var entity = await _repository.GetByEmail(email);
            return _mapper.Map<DummyDTO>(entity);
        }

        public async Task<DummyDTO> Update(DummyDTO DummyDTO)
        {
            var res = await base.Update(DummyDTO);
            return _mapper.Map<DummyDTO>(res);
        }

        public async Task<string> UpdateBulk(List<DummyDTO> DummyDTO)
        {
            var res = await base.UpdateBulk(DummyDTO);
            return res;
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await base.Delete(id);
        }
        public async Task<string> DeleteBulk(List<DummyDTO> DummyDTO)
        {
            var res = await base.DeleteBulk(DummyDTO);
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
        public  async Task<string> CreateBulk(List<DummyDTO> viewModels)
        {
            var res = await base.CreateBulk(viewModels);
            return res;
        }
    }
}
