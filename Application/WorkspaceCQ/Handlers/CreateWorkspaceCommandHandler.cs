using Application.Response;
using Application.WorkspaceCQ.Comands;
using Application.WorkspaceCQ.ViewModels;
using Domain.Abstract;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.WorkspaceCQ.Handlers
{
    public class CreateWorkspaceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.IUserRepository.Get(x => x.Id.ToLower() == request.UserId.ToLower());

            if (user == null)
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    ErrorCode = ErrorCodes.UserNotFound,
                    Message = "Usuário não encontrado.",
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
                ErrorCode = null,
                Message = null,
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
