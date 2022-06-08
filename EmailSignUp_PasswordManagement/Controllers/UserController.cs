using EmailSignUp_PasswordManagement.Repositories.Contracts;
using EmailSignUp_PasswordManagement.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace EmailSignUp_PasswordManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public enum EmailCategories { Registration, ForgetPassword, VerifyEmail }

        public UserController(DataContext context, IUserRepository userRepository, IEmailService emailService)
        {
            _context = context;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if(_userRepository.CheckUserRegistered(request))
            {
                return BadRequest("User already exists.");
            }

            var user = await _userRepository.AddUser(request);

            SendEmail(user, EmailCategories.Registration);

            return Ok("User sucessfully created!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _userRepository.GetUser(request);

            if(user == null)
            {
                return NotFound("User not exists");
            }

            var verifyPasswordHash = VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!verifyPasswordHash)
            {
                return BadRequest("The password is incorrect");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("User not verified");
            }

            return Ok($"Welcome back {user.Email}!");

        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _userRepository.VerifyUser(token);

            if (user == null)
            {
                return BadRequest("Token invalid");
            }

            SendEmail(user, EmailCategories.VerifyEmail);

            return Ok("User verified!");
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userRepository.ForgetPassword(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            SendEmail(user, EmailCategories.ForgetPassword);

            return Ok("You can now reset your password");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userRepository.ResetPassword(request);

            return (user is null) ? BadRequest("Invalid token") : Ok("Password reset successfully");
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void SendEmail(User user, EmailCategories emailCategories)
        {
            var emailDto = new EmailDto();
            emailDto.To = user.Email;

            switch (emailCategories)
            {
                case EmailCategories.Registration:
                    {
                        emailDto.Subject = "Thank you for the registration";
                        emailDto.Body = $"This is your verify token : {user.VerificationToken}";
                        break;
                    }
                case EmailCategories.ForgetPassword:
                    {
                        emailDto.Subject = "You can reset your password now";
                        emailDto.Body = $"This is your reset token : {user.PasswordResetToken}. \n Please reset your password before {user.ResetTokenExpires}";
                        break;
                    }
                case EmailCategories.VerifyEmail:
                    {
                        emailDto.Subject = "Thanks for confirming your email";
                        emailDto.Body = $"Your account has been verified at : {user.VerifiedAt}";
                        break;
                    }
                default:
                    break;
            }
            
            _emailService.SendEmail(emailDto);
        }

    }
}
