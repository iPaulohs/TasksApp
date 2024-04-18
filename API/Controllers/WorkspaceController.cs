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
            var result = await _mediator.Send(command);

            if(result.ErrorCode is null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("Update-Workspace")]
        public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.ErrorCode is null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("Get-Workspace-By-Id")]
        public async Task<IActionResult> GetWorkpaceById([FromQuery] GetWorkspaceByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.ErrorCode is null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("Get-Workspace-By-UserId")]
        public async Task<IActionResult> GetWorkpaceByUserId([FromQuery] GetAllWorkspacesByUserIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.ErrorCode is null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
