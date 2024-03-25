using Application.UserCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create-User")]
        public async Task<ActionResult> CreateUser(CreateUserCommand command)
        {
            var request = await _mediator.Send(command);

            if(request.ErrorCode is null)
            {
                return Ok(request);
            }

            return BadRequest(request);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ErrorCode is null)
            {
                return Ok(request);
            }

            return BadRequest(request);
        }

        [HttpPost("RefreshToken-Token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ErrorCode is null)
            {
                return Ok(request);
            }

            return BadRequest(request);
        }
    }
}
