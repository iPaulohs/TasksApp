using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands
{
    public record RefreshTokenCommand : IRequest<ResponseBase<UserInfoViewModel>>
    {
        public string? ExpiredToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
