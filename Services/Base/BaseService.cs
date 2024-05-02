using AutoMapper;
using DTO.Base;
using Entities.Base;
using Repositories.Base;
using System.Diagnostics;

namespace Services.Base
{
    public abstract class BaseService<TDTO, TEntity> : IBaseService<TDTO> 
        where TDTO : class, IBaseDTO
        where TEntity : class, IBaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity> _repository;
        protected BaseService(IMapper mapper, IBaseRepository<TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public virtual async Task<int> Count()
        {
            return await _repository.Count();
        }
        public virtual async Task<List<TDTO>> GetByListId(List<int> listId)
        {
            var entities = await _repository.GetByListId(listId);
            return _mapper.Map<List<TDTO>>(entities);
        }

        public virtual async Task<List<TDTO>> GetByListProperty(string field, string[] values)
        {
            var entities = await _repository.GetByListProperty(field, values);
            return _mapper.Map<List<TDTO>>(entities);
        }

        public virtual async Task<List<TDTO>> GetAll(int page)
        {
            var entities = await _repository.GetAll(page);
            return _mapper.Map<List<TDTO>>(entities);
        }
        public virtual async Task<List<TDTO>> GetAll()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<TDTO>>(entities);
        }
        public virtual async Task<TDTO> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<TDTO>(entity);
        }
        public virtual async Task<TDTO> Create(TDTO TDto)
        {
            var entity = _mapper.Map<TEntity>(TDto);
            var res = await _repository.Create(entity);
            return _mapper.Map<TDTO>(res);
        }
        public virtual async Task<string> CreateBulk(List<TDTO> viewModels)
        {
            var entities = _mapper.Map<List<TEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.CreateBulk(entities);
            stopwatch.Stop();
            return $"Success create: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public virtual async Task<TDTO> Update(TDTO TDto)
        {
            var entity = _mapper.Map<TEntity>(TDto);
            var res = await _repository.Update(entity);
            return _mapper.Map<TDTO>(res);
        }

        public virtual async Task<string> UpdateBulk(List<TDTO> viewModels)
        {
            var entities = _mapper.Map<List<TEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.UpdateBulk(entities);
            stopwatch.Stop();
            return $"Success update: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }
        public virtual async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        public virtual async Task<string> DeleteBulk(List<TDTO> viewModels)
        {
            var entities = _mapper.Map<List<TEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.DeleteBulk(entities);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public virtual async Task<int> SoftDelete(int id)
        {
            return await _repository.SoftDelete(id);
        }
        public virtual async Task<string> SoftDeleteBulk(List<TDTO> viewModels)
        {
            var entities = _mapper.Map<List<TEntity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.SoftDeleteBulk(entities);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }
    }
}
