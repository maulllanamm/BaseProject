using DTO;
using Entities;
using System.Security.Claims;

namespace Services.Interface
{
    public interface IAuthService
    {
        public Task<OperationResult<GetMeDTO>> GetMe();
        public Task<OperationResult<UserDTO>> Register(RegisterDTO request);
        public Task<OperationResult<UserDTO>> Login(LoginDTO request);
        public Task<OperationResult<string>> ForgotPassword(string email);
        public Task<OperationResult<string>> Verify(string verifyToken);
        public Task<OperationResult<string>> GenerateAccessToken(string username, string roleName);
        public Task<OperationResult<string>> GenerateRefreshToken(string username);
        public Task<OperationResult<string>> GenerateVerifyToken(string email);
        public Task<OperationResult<string>> GeneratePasswordResetToken(string username, string email);
        public void SetRefreshToken(string newRefreshToken, UserDTO user);
        public Task<OperationResult<ClaimsPrincipal>> ValidateAccessToken(string token);
        public Task<OperationResult<bool>> IsRequestPermitted();
    }
}
