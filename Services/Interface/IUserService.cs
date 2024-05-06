using DTO;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> GetAll(int page);
        Task<UserDTO> GetById(Guid id);
        Task<UserDTO> GetByUsername(string username);
        Task<UserDTO> GetByVerifyToken(string verifyToken);
        Task<UserDTO> GetByEmail(string email);
        Task<UserDTO> GetByPasswordResetToken(string passwordToken);
        Task<List<UserDTO>> GetByListId(List<Guid> listId);
        Task<List<UserDTO>> GetByListProperty(string field, string[] values);
        Task<UserDTO> Create(UserDTO userDto);
        Task<UserDTO> Update(UserDTO userDto);
        Task<string> UpdateBulk(List<UserDTO> userDto);
        Task<Guid> Delete(Guid id);
        Task<string> DeleteBulk(List<UserDTO> userDto);
        Task<Guid> SoftDelete(Guid id);
        Task<string> SoftDeleteBulk(List<Guid> listId);
    }
}
