using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using Domain.Entity;
using MediatR;

namespace Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService = authService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validFields = _authService.VerifyUniqueUser(request.Email!, request.Username!);

            if (validFields is Domain.Enum.ValidationFieldsUserEnum.UsernameUnavailable)
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

            if (validFields is Domain.Enum.ValidationFieldsUserEnum.EmailUnavailable)
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

            if (validFields is Domain.Enum.ValidationFieldsUserEnum.UsernameAndEmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = new()
                    {
                        Title = "Email e Username indisponível",
                        StatusMessage = $"O email e o Username já estão sendo utilizados. ",
                        Status = 400
                    },
                    Response = null
                };
            }

            User user = _mapper.Map<User>(request);

            try
            {
                var result = await _unitOfWork.IUserRepository.Create(user);
                _unitOfWork.Commit();

                return new ResponseBase<RefreshTokenViewModel>
                {
                    Info = null,
                    Response = _mapper.Map<RefreshTokenViewModel>(user)
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


