using Application.Response;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    public class GetAllWorkspacesByUserIdQueryHandler(IUnitOfWork iunitOfWork, IMapper mapper): IRequestHandler<GetAllWorkspacesByUserIdQuery, ResponseBase<List<WorkspaceViewModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = iunitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<List<WorkspaceViewModel>>> Handle(GetAllWorkspacesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.IUserRepository.Get(x => x.Id == request.UserId.ToString());

            if(user is null)
            {
                return new ResponseBase<List<WorkspaceViewModel>>
                {
                    ErrorCode = ErrorCodes.UserNotFound,
                    Message = "Usuário não encontrado.",
                    Response = null
                };
            }

            var workspaces = _unitOfWork.IWorkspaceRepository.GetAll(x => x.User == user).ToList();

            return new ResponseBase<List<WorkspaceViewModel>>
            {
                ErrorCode = null,
                Message = null,
                Response = _mapper.Map<List<WorkspaceViewModel>>(workspaces)
            };
        }
    }
}
