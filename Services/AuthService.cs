using AutoMapper;
using DTO;
using Entities;
using Services.Interface;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _role;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserService _user;

        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public AuthService(IRoleService role, IPasswordHasher passwordHasher, IUserService user, IMapper mapper)
        {
            _role = role;
            _passwordHasher = passwordHasher;
            _user = user;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDTO>> Register(RegisterDTO request)
        {
            var role = await _role.GetById(request.RoleId);
            if(role is null)
            {
                return OperationResult<UserDTO>.Failure("Role not found.");
            }
            var salt = _passwordHasher.GenerateSalt();
            var user = new UserDTO
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Username = request.Username,
                Email = request.Email,
                PasswordSalt = salt,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                PasswordHash = _passwordHasher.ComputeHash(request.Password, salt, _papper, _iteration)
            };

            user = await _user.Create(user);
            var res = _mapper.Map<UserDTO>(user);
            res.Password = "==HASH==";
            return OperationResult<UserDTO>.Success(res);
        }

        public async Task<OperationResult<UserDTO>> Login(LoginDTO request)
        {
            var user = await _user.GetByUsername(request.Username);
            if (user == null)
            {
                return OperationResult<UserDTO>.Failure("Username not found");
            }

            var passwordHash = _passwordHasher.ComputeHash(request.Password, user.PasswordSalt, _papper, _iteration);
            if (passwordHash != user.PasswordHash)
            {
                return OperationResult<UserDTO>.Failure("Incorrect password");
            }
            return OperationResult<UserDTO>.Success(user);
        }
    }
}
