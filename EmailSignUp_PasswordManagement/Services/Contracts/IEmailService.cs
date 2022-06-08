namespace EmailSignUp_PasswordManagement.Services.Contracts
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
