using DTO;

namespace Services
{
    public interface IUserService
    {
        Task<Guid> Delete(Guid id);
        Task<string> DeleteBulk(List<UserDTO> userDto);
        Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> GetAll(int page);
        Task<UserDTO> GetById(Guid id);
        Task<List<UserDTO>> GetByListId(List<Guid> listId);
        Task<List<UserDTO>> GetByListProperty(string field, string[] values);
        Task<Guid> SoftDelete(Guid id);
        Task<string> SoftDeleteBulk(List<Guid> listId);
        Task<UserDTO> Update(UserDTO userDto);
        Task<string> UpdateBulk(List<UserDTO> userDto);
    }
}
