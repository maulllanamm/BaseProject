using AutoMapper;
using DTO.Base;
using Entities.Base;
using Repositories.Base;
using System.Diagnostics;

namespace Services.Base
{
    public abstract class BaseGuidService<TGuidDTO, TGuidEntity> : IBaseGuidService<TGuidDTO>
        where TGuidDTO : class, IBaseGuidDTO
        where TGuidEntity : class, IBaseGuidEntity
    {
        private readonly IMapper _mapper;
        private readonly IBaseGuidRepository<TGuidEntity> _repository;
        protected BaseGuidService(IMapper mapper, IBaseGuidRepository<TGuidEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public virtual async Task<int> Count()
        {
            return await _repository.Count();
        }
        public virtual async Task<List<TGuidDTO>> GetByListId(List<Guid> listId)
        {
            var entities = await _repository.GetByListId(listId);
            return _mapper.Map<List<TGuidDTO>>(entities);
        }

        public virtual async Task<List<TGuidDTO>> GetByListProperty(string field, string[] values)
        {
            var entities = await _repository.GetByListProperty(field, values);
            return _mapper.Map<List<TGuidDTO>>(entities);
        }

        public virtual async Task<List<TGuidDTO>> GetAll(int page)
        {
            var entities = await _repository.GetAll(page);
            return _mapper.Map<List<TGuidDTO>>(entities);
        }
        public virtual async Task<List<TGuidDTO>> GetAll()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<TGuidDTO>>(entities);
        }
        public virtual async Task<TGuidDTO> GetById(Guid id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<TGuidDTO>(entity);
        }
        public virtual async Task<TGuidDTO> Create(TGuidDTO TGuidDTO)
        {
            var entity = _mapper.Map<TGuidEntity>(TGuidDTO);
            var res = await _repository.Create(entity);
            return _mapper.Map<TGuidDTO>(res);
        }
        public virtual async Task<string> CreateBulk(List<TGuidDTO> viewModels)
        {
            var entities = _mapper.Map<List<TGuidEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.CreateBulk(entities);
            stopwatch.Stop();
            return $"Success create: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public virtual async Task<TGuidDTO> Update(TGuidDTO TGuidDTO)
        {
            var entity = _mapper.Map<TGuidEntity>(TGuidDTO);
            var res = await _repository.Update(entity);
            return _mapper.Map<TGuidDTO>(res);
        }

        public virtual async Task<string> UpdateBulk(List<TGuidDTO> viewModels)
        {
            var entities = _mapper.Map<List<TGuidEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.UpdateBulk(entities);
            stopwatch.Stop();
            return $"Success update: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }
        public virtual async Task<Guid> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }
        public virtual async Task<string> DeleteBulk(List<TGuidDTO> viewModels)
        {
            var entities = _mapper.Map<List<TGuidEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.DeleteBulk(entities);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public virtual async Task<Guid> SoftDelete(Guid id)
        {
            return await _repository.SoftDelete(id);
        }
        public virtual async Task<string> SoftDeleteBulk(List<Guid> listId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.SoftDeleteBulk(listId);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }
    }
}