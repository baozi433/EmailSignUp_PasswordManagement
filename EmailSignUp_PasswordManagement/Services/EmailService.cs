using EmailSignUp_PasswordManagement.Services.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailSignUp_PasswordManagement.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(EmailDto request)
        {
            //Set up email
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email").GetSection("Username").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            //Set up smtp service
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("Email").GetSection("Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("Email").GetSection("Username").Value, _configuration.GetSection("Email").GetSection("Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
