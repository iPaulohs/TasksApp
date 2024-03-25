using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Abstract;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers
{
    public class LoginUserCommandHandler(IAuthService authService, SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration) : IRequestHandler<LoginUserCommand, ResponseBase<UserInfoViewModel>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IAuthService _authService = authService;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        public async Task<ResponseBase<UserInfoViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);

            if (user is null)
            {
                return new ResponseBase<UserInfoViewModel>
                {
                    ErrorCode = ErrorCodes.LoginFailed,
                    Message = "Usuário não encontrado.",
                    Response = null
                };
            }

            var passwordHash = _authService.HashingUserPassword(request.Password!);
            var refreshToken = _authService.GenerateRefreshJWT();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            var result = await _signInManager.PasswordSignInAsync(user, passwordHash, false, false);

            if (result.Succeeded)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpirationTime = DateTime.Now.AddSeconds(25);
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
                        RefreshToken = refreshToken
                    }
                };
            }

            return new ResponseBase<UserInfoViewModel>
            {
                ErrorCode = ErrorCodes.LoginFailed,
                Message = "O login falhou. Verifique os dados e tente novamente.",
                Response = null
            }; ;
        }
    }
}
