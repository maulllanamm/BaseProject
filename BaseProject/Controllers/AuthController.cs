using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;

namespace BaseProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService service, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMe()
        {
            var user = await _service.GetMe();
            return Ok(user.Data);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO request)
        {
            var result = await _service.Register(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO request)
        {
            var login = await _service.Login(request);
            if (!login.IsSuccess)
            {
                return BadRequest(login.ErrorMessage);
            }
            var accessToken = await _service.GenerateAccessToken(login.Data.Username, login.Data.RoleName);
            var refreshToken = await _service.GenerateRefreshToken(login.Data.Username);
            _service.SetRefreshToken(refreshToken.Data, login.Data);
            return Ok(accessToken.Data);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Invalid token.");
            }

            var principal = await _service.ValidateAccessToken(refreshToken);
            if (!principal.IsSuccess)
            {
                return NotFound(principal.ErrorMessage);
            }

            // Mendapatkan informasi user dari token
            var username = principal.Data.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetByUsername(username);
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return BadRequest("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }
            var newToken = await _service.GenerateAccessToken(user.Username, user.RoleName);
            var newRefreshToken = await _service.GenerateRefreshToken(user.Username);
            _service.SetRefreshToken(newRefreshToken.Data, user);
            return Ok(newToken);
        }
    }
}
