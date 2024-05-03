using DTO.Base;

namespace Services.Base
{
    public interface IBaseService<TDTO> where TDTO : class, IBaseDTO 
    {
        public Task<List<TDTO>> GetAll();
        public Task<List<TDTO>> GetAll(int page);
        public Task<TDTO> GetById(int id);
        public Task<List<TDTO>> GetByListId(List<int> listId);
        public Task<List<TDTO>> GetByListProperty(string field, string[] values);
        public Task<TDTO> Create(TDTO TDto);
        public Task<string> CreateBulk(List<TDTO> viewModels);
        public Task<TDTO> Update(TDTO TDto);
        public Task<string> UpdateBulk(List<TDTO> viewModels);
        public Task<int> Delete(int id);
        public Task<string> DeleteBulk(List<TDTO> viewModels);
        public Task<int> SoftDelete(int id);
        public Task<string> SoftDeleteBulk(List<int> listId);
        public Task<int> Count();
    }
}
