namespace EmailSignUp_PasswordManagement.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUser(UserLoginRequest request);
        bool CheckUserRegistered(UserRegisterRequest request);
        Task<User> AddUser(UserRegisterRequest request);
        Task<User> VerifyUser(string token);
        Task<User> ForgetPassword(string email);
        Task<User> ResetPassword(ResetPasswordRequest request);
    }
}
