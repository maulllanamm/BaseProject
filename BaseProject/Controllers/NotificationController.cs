using DTO;
using DTO.Base;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Hub;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaseProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContex;

        public NotificationController(IAuthService service, IUserService userService, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContex)
        {
            _service = service;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _hubContex = hubContex;
        }

        [HttpPost]
        public async Task<ActionResult> BroadcastToAll(string message)
        {
            await _hubContex.Clients.All.SendAsync("SendMessage", message);
            return Ok("success");
        }

        
    }
}
