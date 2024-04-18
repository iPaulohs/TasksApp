using Domain.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Auth
{
    public class AuthService(IConfiguration configuration, IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public string GenerateJWT(string email, string Username)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = _configuration["JWT:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("emailAdress", email),
                new("Username", Username),
                new("emailIdentifier", email.Split("@").ToString()!),
                new("currentTime", DateTime.Now.ToString())
            };

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.Now.AddHours(48), signingCredentials: credentials, claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshJWT()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }


        public bool GetTokenIsValid(string username)
        {
            return _unitOfWork.IUserRepository.Get(x => x.Username == username) is null;
        }

        public string HashingUserPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); //x2 faz a conversão do valor em uma representação hexadecimal
            }

            return builder.ToString();
        }

        public bool VerifyUniqueUsername(string userName)
        {
            return _unitOfWork.IUserRepository.Get(x => x.Username == userName) is null;
        }

        public bool VerifyUniqueEmail(string email)
        {
            return _unitOfWork.IUserRepository.Get(x => x.Email == email) is null;
        }
    }
}
