﻿using AutoMapper;
using DTO;
using DTO.Base;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserService _user;
        private readonly IRoleService _role;
        private readonly IPermissionService _permission;
        private readonly IRolePermissionService _rolePermission;
        private readonly JwtManagement _jwt;

        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public AuthService(IRoleService role, IPasswordHasher passwordHasher, IUserService user, IMapper mapper, IOptions<JwtManagement> jwt, IHttpContextAccessor httpCont, IPermissionService permission = null, IRolePermissionService rolePermission = null)
        {
            _role = role;
            _passwordHasher = passwordHasher;
            _user = user;
            _mapper = mapper;
            _jwt = jwt.Value;
            _httpCont = httpCont;
            _permission = permission;
            _rolePermission = rolePermission;
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
            if (role is null)
            {
                return OperationResult<UserDTO>.Failure("Role not found.");
            }
            var salt = _passwordHasher.GenerateSalt();
            var verifyToken = await GenerateVerifyToken(request.Email);
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
                PasswordHash = _passwordHasher.ComputeHash(request.Password, salt, _papper, _iteration),
                VerifyToken = verifyToken.Data,
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

            if(user.VerifyDate is null)
            {
                return OperationResult<UserDTO>.Failure("User is not verified");
            }
            return OperationResult<UserDTO>.Success(user);
        }

        public async Task<OperationResult<string>> Verify(string verifyToken)
        {
            var user = await _user.GetByVerifyToken(verifyToken);
            if(user is null)
            {
                return OperationResult<string>.Failure("Invalid token");
            }
            user.VerifyDate = DateTime.UtcNow;
            await _user.Update(user);
            return OperationResult<string>.Success("User verified");
        }

        public async Task<OperationResult<string>> ForgotPassword(string email)
        {
            var user = await _user.GetByEmail(email);
            if (user is null)
            {
                return OperationResult<string>.Failure("User not found");
            }
            var passwordResetToken = await GeneratePasswordResetToken(user.Username, user.Email);
            user.PasswordResetToken = passwordResetToken.Data;

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(passwordResetToken.Data) as JwtSecurityToken;

            user.PasswordResetExpires = securityToken.ValidTo;
            await _user.Update(user);

            return OperationResult<string>.Success("You may now reset your password");
        }

        public async Task<OperationResult<string>> ResetPassword(ResetPasswordDTO request)
        {
            var user = await _user.GetByPasswordResetToken(request.PasswordResetToken);
            if (user is null)
            {
                return OperationResult<string>.Failure("Invalid token");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(request.PasswordResetToken) as JwtSecurityToken;

            if (securityToken.ValidTo < user.PasswordResetExpires)
            {
                return OperationResult<string>.Failure("Token expired");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return OperationResult<string>.Failure("Password doesn't match");
            }
            var newPassword = _passwordHasher.ComputeHash(request.NewPassword, user.PasswordSalt, _papper, _iteration);
            user.PasswordHash = newPassword;
            user.PasswordResetToken = null;
            user.PasswordResetExpires = null;

            await _user.Update(user);

            return OperationResult<string>.Success("Password successfully reset");
        }

        public async Task<OperationResult<string>> GeneratePasswordResetToken(string username , string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);
            return OperationResult<string>.Success(token);
        }

        public async Task<OperationResult<string>> GenerateVerifyToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);
            return OperationResult<string>.Success(token);
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
            user.RefreshTokenCreated = DateTime.UtcNow;
            user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes);

            _user.Update(user);
        }

        public async Task<OperationResult<ClaimsPrincipal>> ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = key,
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);


            var jsonToken = validatedToken as JwtSecurityToken;

            var isValidToken = jsonToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);
            if (jsonToken == null || !isValidToken)
            {
                return OperationResult<ClaimsPrincipal>.Failure("Invalid token");
            }
            return OperationResult<ClaimsPrincipal>.Success(principal);
        }

        public async Task<OperationResult<bool>> IsRequestPermitted()
        {
            var getMe = await GetMe();
            var username = getMe.Data.Username;
            var role = getMe.Data.Role;
            var path = _httpCont.HttpContext.Request.Path.Value;
            var method = _httpCont.HttpContext.Request.Method.ToString();

            if (role == "Administrator")
            {
                return OperationResult<bool>.Success(true);
            }

            var roles = await _role.GetAll();
            var permissions = await _permission.GetAll();
            var rolePermissions = await _rolePermission.GetAll();

            var myRole = roles.FirstOrDefault(x => x.Name == role);
            if (myRole == null)
            {
                return OperationResult<bool>.Success(true);
            }
            var myRolePermissions = rolePermissions.Where(x => x.role_id == myRole.Id).Select(x => x.permission_id).ToList();
            var myPermissions = permissions.Where(x => myRolePermissions.Contains(x.Id));

            var checkPermission = myPermissions.FirstOrDefault(x => x.HttpMethod == method && x.Path == path);
            if (checkPermission == null)
            {
                return OperationResult<bool>.Failure("You don't have permission");
            }
            return OperationResult<bool>.Success(true);

        }
    }
}
