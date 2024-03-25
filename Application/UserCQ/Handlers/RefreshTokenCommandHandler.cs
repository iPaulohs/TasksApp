using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Abstract;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Application.UserCQ.Handlers
{
    public class RefreshTokenCommandHandler(IAuthService authService, IConfiguration configuration, UserManager<User> userManager) : IRequestHandler<RefreshTokenCommand, ResponseBase<UserInfoViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<ResponseBase<UserInfoViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string accessToken = request.ExpiredToken!;
            string refreshToken = request.RefreshToken!;

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken, _configuration);

            if(principal is null)
            {
                return new ResponseBase<UserInfoViewModel>
                {
                    ErrorCode = ErrorCodes.PrincipalNotFound,
                    Message = "Token inválido para a operaçao de refresh token.",
                    Response = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(accessToken);

            string username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Username" )?.Value;

            Console.WriteLine(username);

            var user = await _userManager.FindByNameAsync(username!);

            if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpirationTime >= DateTime.Now)
            {
                return new ResponseBase<UserInfoViewModel>
                {
                    ErrorCode = ErrorCodes.InvalidTokenRefreshToken,
                    Message = "Token inválido ou nao expirado para a operaçao de refresh token.",
                    Response = null
                };
            }

            var newRefreshToken = _authService.GenerateRefreshJWT();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ResponseBase<UserInfoViewModel>
            {
                ErrorCode = null,
                Message = null,
                Response = new UserInfoViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.UserName,
                    Token = _authService.GenerateJWT(user.Email!, user.UserName!),
                    RefreshToken = newRefreshToken
                }
            };
        }
    }
}
