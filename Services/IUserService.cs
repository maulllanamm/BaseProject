using DTO;

namespace Services
{
    public interface IUserService
    {
        Task<int> Delete(int id);
        Task<string> DeleteBulk(List<UserDTO> userDto);
        Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> GetAll(int page);
        Task<UserDTO> GetById(int id);
        Task<List<UserDTO>> GetByListId(List<int> listId);
        Task<List<UserDTO>> GetByListProperty(string field, string[] values);
        Task<int> SoftDelete(int id);
        Task<string> SoftDeleteBulk(List<int> listId);
        Task<UserDTO> Update(UserDTO userDto);
        Task<string> UpdateBulk(List<UserDTO> userDto);
    }
}
