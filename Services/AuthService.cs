using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpCont;
        private readonly IRoleService _role;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserService _user;
        private readonly JwtDTO _jwt;

        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public AuthService(IRoleService role, IPasswordHasher passwordHasher, IUserService user, IMapper mapper, IOptions<JwtDTO> jwt, IHttpContextAccessor httpCont)
        {
            _role = role;
            _passwordHasher = passwordHasher;
            _user = user;
            _mapper = mapper;
            _jwt = jwt.Value;
            _httpCont = httpCont;
        }

        public async Task<OperationResult<GetMeDTO>> GetMe()
        {
            if (_httpCont.HttpContext == null)
            {
                return OperationResult<GetMeDTO>.Failure("Non Authorized");

            }
            var username = _httpCont.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var role = _httpCont.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = new GetMeDTO
            {
                Username = username,
                Role = role,
            };

            return OperationResult<GetMeDTO>.Success(result);
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

        public async Task<OperationResult<string>> GenerateAccessToken(string username, string roleName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleName.ToString()),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryAccessMinutes),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);
            return OperationResult<string>.Success(token);
        }

        public async Task<OperationResult<string>> GenerateRefreshToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);
            return OperationResult<string>.Success(token);
        }

        public void SetRefreshToken(string newRefreshToken, UserDTO user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes)
            };
            _httpCont.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            user.RefreshToken = newRefreshToken;
            user.TokenCreated = DateTime.UtcNow;
            user.TokenExpires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes);

            _user.Update(user);
        }
    }
}
