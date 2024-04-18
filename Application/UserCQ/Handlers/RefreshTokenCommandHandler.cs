using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Abstract;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers
{
    public class RefreshTokenCommandHandler(IAuthService authService, IConfiguration configuration, IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.IUserRepository.Get(x => x.Username == request.Username);

            if (user is null || user.RefreshToken != request.RefreshToken! || user.RefreshTokenExpirationTime < DateTime.Now)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Token inválido",
                        StatusMessage = $"Refresh Token inválido ou expirado. Faça login novamente.",
                        Status = 400
                    },
                    Response = null
                };
            }

            user.RefreshToken = _authService.GenerateRefreshJWT();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await _unitOfWork.IUserRepository.Update(user);
            _unitOfWork.CommitAsync();

            return new ResponseBase<RefreshTokenViewModel>
            {
                Info = null,
                Response = new RefreshTokenViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.Username,
                    RefreshToken = user.RefreshToken,
                    Token = _authService.GenerateJWT(user.Email!, user.Username!)
                }
            };
        }
    }
}
