using EmailSignUp_PasswordManagement.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailSignUp_PasswordManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]

        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok($"Email sent to {request.To}");
        }
    }
}
