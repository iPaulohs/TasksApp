using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Comands
{
    public record UpdateWorkspaceCommand : IRequest<ResponseBase<WorkspaceViewModel>>
    {
        public string? Title { get; set; }
        public Guid Id { get; set; }
        public bool Arquived { get; set; }
    }
}
