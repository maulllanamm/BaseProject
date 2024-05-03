using DTO;

namespace Services.Interface
{
    public interface IAuthService
    {
        public Task<OperationResult<UserDTO>> Register(RegisterDTO request);
        public Task<OperationResult<UserDTO>> Login(LoginDTO request);
    }
}
