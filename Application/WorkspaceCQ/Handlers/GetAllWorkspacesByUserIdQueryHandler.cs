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
            var user = _unitOfWork.IUserRepository.Get(x => x.Id == request.UserId);

            if(user is null)
            {
                return new()
                {
                    Info = new()
                    {
                        Title = "Usuário não encontrado",
                        StatusMessage = "Nenhum usuário encontrado com o Id informado.",
                        Status = 404
                    },
                    Response = null
                };
            }

            var workspaces = _unitOfWork.IWorkspaceRepository.GetAll(x => x.User == user).ToList();

            return new ResponseBase<List<WorkspaceViewModel>>
            {
                Info = null,
                Response = _mapper.Map<List<WorkspaceViewModel>>(workspaces)
            };
        }
    }
}
