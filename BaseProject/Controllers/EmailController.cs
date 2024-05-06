using DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace BaseProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(EmailDTO email)
        {
            _emailService.SendEmail(email);
            return Ok();
        }
    }
}
