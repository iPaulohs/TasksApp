using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings()
        {
            CreateMap<Workspace, WorkspaceViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(x => x.User.Id));
        }
    }
}
