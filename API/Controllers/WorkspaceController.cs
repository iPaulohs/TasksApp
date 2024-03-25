using Application.WorkspaceCQ.Comands;
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
            var result = await _mediator.Send(command);

            if(result.ErrorCode == 0)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("Update-Workspace")]
        public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.ErrorCode == 0)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
