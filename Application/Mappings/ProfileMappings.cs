using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings([FromServices] IAuthService authService, IConfiguration configuration)
        {
            CreateMap<Workspace, WorkspaceViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(x => x.User!.Id));
            CreateMap<RefreshTokenViewModel, UserInfoViewModel>();

            _ = int.TryParse(configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            CreateMap<CreateUserCommand, User>()
                .ForMember(x => x.PasswordHash, x => x.MapFrom(x => authService.HashingUserPassword(x.Password!)))
                .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => DateTime.Now.AddDays(refreshTokenValidityInMinutes)));

            CreateMap<User, RefreshTokenViewModel>()
                .ForMember(x => x.Token, x => x.MapFrom(x => authService.GenerateJWT(x.Email!, x.Username!)));
        }
    }
}
