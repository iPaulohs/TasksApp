using Domain.Enum;

namespace Domain.Abstract
{
    public interface IAuthService
    {
        public string GenerateJWT(string email, string Username);
        public string HashingUserPassword(string password);
        public ValidationFieldsUserEnum VerifyUniqueUser(string email, string userName);
        public string GenerateRefreshJWT();
        bool GetTokenIsValid(string username);
    }
}
