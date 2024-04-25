using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers
{
    public class LoginUserCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.IUserRepository.Get(x => x.Email!.ToLower() == request.Email!.ToLower());

            if (user is null)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Usuário não encontrado",
                        StatusMessage = $"Não foi encontrado nenhum usuário com o 'email' {request.Email}",
                        Status = 400
                    },
                    Response = null
                };
            }

            var passwordHash = _authService.HashingUserPassword(request.Password!);
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            var result = user.PasswordHash == passwordHash;

            if (result)
            {
                user.RefreshToken = _authService.GenerateRefreshJWT();
                user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                await _unitOfWork.IUserRepository.Update(user);
                _unitOfWork.Commit();

                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = null,
                    Response = _mapper.Map<RefreshTokenViewModel>(user)
                };
            }

            return new ResponseBase<RefreshTokenViewModel>
            {
                Info = new()
                {
                    Title = "Falha no login",
                    StatusMessage = $"Email ou senha incorretos.",
                    Status = 500
                },
                Response = null
            }; ;
        }
    }
}
