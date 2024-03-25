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
    public class CreateUserCommandHandler(IAuthService authService, UserManager<User> userManager, IConfiguration configuration) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ResponseBase<UserInfoViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var UsernameUnique = _authService.VerifyUniqueUsername(request.Username!);

            var emailUnique = _authService.VerifyUniqueEmail(request.Email!);

            if (!UsernameUnique)
            {
                return new ResponseBase<UserInfoViewModel>
                {
                    ErrorCode = ErrorCodes.EmailIsNotAvailable,
                    Message = null,
                    Response = null
                };
            }

            if (!emailUnique)
            {
                return new ResponseBase<UserInfoViewModel>
                {
                    ErrorCode = ErrorCodes.UsernameIsNotAvailable,
                    Message = null,
                    Response = null
                };
            }

            var passwordHash = _authService.HashingUserPassword(request.Password!);
            var refreshToken = _authService.GenerateRefreshJWT();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
            
            User user = new()
            {
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.Username,
                Email = request.Email,
                RefreshToken = refreshToken,
                RefreshTokenExpirationTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes)
            };

            var result = await _userManager.CreateAsync(user, passwordHash);

            if (result.Succeeded)
            {
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
                ErrorCode = ErrorCodes.UserNotCreated,
                Message = "Falha no registro do usuário. Tente novaamente.",
                Response = null
            };
        }
    }
}
