using DTO;
using Entities;

namespace Services.Interface
{
    public interface IAuthService
    {
        public Task<OperationResult<GetMeDTO>> GetMe();
        public Task<OperationResult<UserDTO>> Register(RegisterDTO request);
        public Task<OperationResult<UserDTO>> Login(LoginDTO request);
        public Task<OperationResult<string>> GenerateAccessToken(string username, string roleName);
        public Task<OperationResult<string>> GenerateRefreshToken(string username);
        public void SetRefreshToken(string newRefreshToken, UserDTO user);
    }
}
