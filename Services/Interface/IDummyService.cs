using DTO;

namespace Services.Interface
{
    public interface IDummyService
    {
        Task<List<DummyDTO>> GetAll();
        Task<List<DummyDTO>> GetAll(int page);
        Task<DummyDTO> GetById(Guid id);
        Task<DummyDTO> GetByUsername(string username);
        Task<DummyDTO> GetByEmail(string email);
        Task<List<DummyDTO>> GetByListId(List<Guid> listId);
        Task<List<DummyDTO>> GetByListProperty(string field, string[] values);
        Task<DummyDTO> Create(DummyDTO DummyUser);
        Task<DummyDTO> Update(DummyDTO DummyUser);
        Task<string> UpdateBulk(List<DummyDTO> DummyUser);
        Task<Guid> Delete(Guid id);
        Task<string> DeleteBulk(List<DummyDTO> DummyUser);
        Task<Guid> SoftDelete(Guid id);
        Task<string> SoftDeleteBulk(List<Guid> listId);
        Task<string> CreateBulk(List<DummyDTO> viewModels);
    }
}
