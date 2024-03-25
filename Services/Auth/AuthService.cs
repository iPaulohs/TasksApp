using Domain.Abstract;
using Infra.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Auth
{
    public class AuthService(IConfiguration configuration, TasksDbContext context) : IAuthService
    {
        private readonly TasksDbContext _context = context;
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
            new("email", email),
            new("Username", Username),
            new("aleatoryGuid", Guid.NewGuid().ToString()),
            new("aleatoryTime", DateTime.Now.ToString())
        };

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.Now.AddHours(24), signingCredentials: credentials, claims: claims);
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


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
        {
            var key = configuration["JWT:Key"] ?? throw new InvalidOperationException("Invalid key");

            var tokenParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToekn || !jwtSecurityToekn.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public string HashingUserPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); //x2 faz a conversão do valor em uma representação hexadecimal
                }

                return builder.ToString();
            }
        }

        public bool VerifyUniqueUsername(string userName)
        {
            var result = _context.Users.Any(x => x.UserName == userName);
            return result == false;
        }

        public bool VerifyUniqueEmail(string email)
        {
            var result = _context.Users.Any(x => x.Email == email);
            return result == false;
        }
    }
}
