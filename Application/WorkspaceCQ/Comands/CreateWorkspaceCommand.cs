using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Comands
{
    public record CreateWorkspaceCommand : IRequest<ResponseBase<WorkspaceViewModel>>
    {
        public string? Title { get; set; }
        public Guid UserId { get; set; }
    }
}
