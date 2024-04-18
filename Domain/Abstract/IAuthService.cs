namespace Domain.Abstract
{
    public interface IAuthService
    {
        public string GenerateJWT(string email, string Username);
        public string HashingUserPassword(string password);
        public bool VerifyUniqueUsername(string Username);
        public bool VerifyUniqueEmail(string email);
        public string GenerateRefreshJWT();
        bool GetTokenIsValid(string username);
    }
}
