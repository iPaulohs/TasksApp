using Application.Response;
using Application.WorkspaceCQ.Comands;
using Application.WorkspaceCQ.ViewModels;
using Domain.Abstract;
using Domain.Entity;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    public class CreateWorkspaceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.IUserRepository.Get(x => x.Id == request.UserId);

            if (user is null)
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    Info = new()
                    {
                        Title = "Usuário não encontrado",
                        StatusMessage = $"Não foi encontrado nenhum usuário com o 'Id' {request.UserId}",
                        Status = 400
                    },
                    Response = null
                };
            }

            Workspace workspace = new()
            {
                Title = request.Title,
                Arquived = false,
                User = user
            };

            await _unitOfWork.IWorkspaceRepository.Create(workspace);
            _unitOfWork.CommitAsync();

            return new ResponseBase<WorkspaceViewModel>
            {
                Info = null,
                Response = new WorkspaceViewModel
                {
                    Title = workspace.Title,
                    Id = workspace.Id,
                    List = [],
                    UserId = user.Id!
                }
            };
        }
    }
}
