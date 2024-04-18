using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Queries
{
    public record GetAllWorkspacesByUserIdQuery : IRequest<ResponseBase<List<WorkspaceViewModel>>>
    {
        public Guid UserId { get; set; }
    }
}
