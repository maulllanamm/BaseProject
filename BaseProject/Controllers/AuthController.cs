using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

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
            return Ok(login.Data);
        }
    }
}
