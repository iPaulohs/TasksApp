using Application.Response;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    public class GetWorkspaceByIdQueryHandler(IUnitOfWork iunitOfWork, IMapper mapper) : IRequestHandler<GetWorkspaceByIdQuery, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _iunitOfWork = iunitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
        {
            var workspace = _iunitOfWork.IWorkspaceRepository.Get(x => x.Id == request.Id);

            if (workspace == null)
            {
                return new()
                {
                    Info = new()
                    {
                        Title = "Workspace não encontrado",
                        StatusMessage = "Nenhum Workspace encontrado com o Id informado.",
                        Status = 404
                    },
                    Response = null
                };
            }

            return new ResponseBase<WorkspaceViewModel>
            {
                Info = null,
                Response = _mapper.Map<WorkspaceViewModel>(workspace)
            };
        }
    }
}
