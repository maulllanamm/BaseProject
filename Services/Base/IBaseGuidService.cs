using DTO.Base;

namespace Services.Base
{
    public interface IBaseGuidService<TGuidDTO> where TGuidDTO : class, IBaseGuidDTO
    {
        public Task<List<TGuidDTO>> GetAll();
        public Task<List<TGuidDTO>> GetAll(int page);
        public Task<TGuidDTO> GetById(Guid id);
        public Task<List<TGuidDTO>> GetByListId(List<Guid> listId);
        public Task<List<TGuidDTO>> GetByListProperty(string field, string[] values);
        public Task<TGuidDTO> Create(TGuidDTO TGuidDTO);
        public Task<string> CreateBulk(List<TGuidDTO> viewModels);
        public Task<TGuidDTO> Update(TGuidDTO TGuidDTO);
        public Task<string> UpdateBulk(List<TGuidDTO> viewModels);
        public Task<Guid> Delete(Guid id);
        public Task<string> DeleteBulk(List<TGuidDTO> viewModels);
        public Task<Guid> SoftDelete(Guid id);
        public Task<string> SoftDeleteBulk(List<Guid> listId);
        public Task<int> Count();
    }
}
