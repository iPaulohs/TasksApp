using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Abstract;
using Domain.Entity;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IConfiguration configuration) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var UsernameUnique = _authService.VerifyUniqueUsername(request.Username!);

            var emailUnique = _authService.VerifyUniqueEmail(request.Email!);

            if (!UsernameUnique)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Username indisponível",
                        StatusMessage = $"O username já está sendo utilizado. ",
                        Status = 400
                    },
                    Response = null
                };
            }

            if (!emailUnique)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Email indisponível",
                        StatusMessage = $"O email já está sendo utilizado. ",
                        Status = 400
                    },
                    Response = null
                };
            }

            var refreshToken = _authService.GenerateRefreshJWT();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            User user = new()
            {
                Name = request.Name,
                Surname = request.Surname,
                Username = request.Username,
                Email = request.Email,
                RefreshToken = refreshToken,
                PasswordHash = _authService.HashingUserPassword(request.Password!),
                RefreshTokenExpirationTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes)
            };

            try
            {
                var result = await _unitOfWork.IUserRepository.Create(user);
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
                        Token = _authService.GenerateJWT(user.Email!, user.Username!),
                        RefreshToken = user.RefreshToken
                    }
                };
            }
            catch (Exception)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Erro no servidor",
                        StatusMessage = $"Ocorreu um erro no servidor ao registrar o usuário. Tente novamente mais tarde.",
                        Status = 500
                    },
                    Response = null
                };

                throw;
            }
        }
    }
}


