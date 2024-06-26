﻿using Application.Response;
using Application.WorkspaceCQ.Comands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Abstract;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    public class UpdateWorkspaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var workspace = _unitOfWork.IWorkspaceRepository
                .GetWorkspaceIncludeUser(x => x.Id == request.Id, x => x.Id == request.Id);

            if (workspace == null)
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    Info = new()
                    {
                        Title = "Workpace não encontrado",
                        StatusMessage = $"Não foi encontrado nenhum workspace com o 'Id' {request.Id}",
                        Status = 400
                    },
                    Response = null
                };
            }

            workspace.Title = request.Title;
            workspace.Arquived = request.Arquived;

            await _unitOfWork.IWorkspaceRepository.Update(workspace);
            _unitOfWork.Commit();
            var _workspace = _mapper.Map<WorkspaceViewModel>(workspace);

            return new ResponseBase<WorkspaceViewModel>
            {
                Info = null,
                Response = _workspace
            };
        }
    }
}
