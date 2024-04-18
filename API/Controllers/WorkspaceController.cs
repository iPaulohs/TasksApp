using Application.WorkspaceCQ.Comands;
using Application.WorkspaceCQ.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkspaceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create-Workspace")]
        public async Task<IActionResult> CreateWorkspace(CreateWorkspaceCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.Info is null)
            {
                return Ok(request.Response);
            }

            return BadRequest(request.Info);
        }

        [HttpPost("Update-Workspace")]
        public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.Info is null)
            {
                return Ok(request.Response);
            }

            return BadRequest(request.Info);
        }
    }
}
