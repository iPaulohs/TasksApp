using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController(IMediator mediator, IConfiguration configuration, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        [HttpPost("Create-User")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>?>> CreateUser(CreateUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.Info is null)
            {
                var userInfo = request.Response;

                if (userInfo != null)
                {
                    var cookieOptionsToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(48)
                    };

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);


                    var cookieOptionsRefreshToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenValidityInDays)
                    };


                    Response.Cookies.Append("jwt", request.Response!.Token!, cookieOptionsToken);
                    Response.Cookies.Append("refreshToken", request.Response!.RefreshToken!, cookieOptionsRefreshToken);
                    return Ok(new ResponseBase<UserInfoViewModel>
                    {
                        Info = null,
                        Response = _mapper.Map<UserInfoViewModel>(request.Response)
                    });
                }
            }

            return BadRequest(new ResponseBase<UserInfoViewModel>
            {
                Info = request.Info,
                Response = null
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>?>> Login(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.Info is null)
            {
                var userInfo = request.Response;

                if (userInfo != null)
                {
                    var cookieOptionsToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(48)
                    };

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);


                    var cookieOptionsRefreshToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenValidityInDays)
                    };


                    Response.Cookies.Append("jwt", request.Response!.Token!, cookieOptionsToken);
                    Response.Cookies.Append("refreshToken", request.Response!.RefreshToken!, cookieOptionsRefreshToken);
                    return Ok(new ResponseBase<UserInfoViewModel>
                    {
                        Info = null,
                        Response = _mapper.Map<UserInfoViewModel>(request.Response)
                    });
                }
            }

            return BadRequest(new ResponseBase<UserInfoViewModel>
            {
                Info = request.Info,
                Response = null
            });
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>?>> RefreshToken(RefreshTokenCommand command)
        {
            var request = await _mediator.Send(new RefreshTokenCommand
            {
                Username = command.Username,
                RefreshToken = Request.Cookies["refreshToken"]
            });

            if (request.Info is null)
            {
                var cookieOptionsToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(48)
                };

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);


                var cookieOptionsRefreshToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenValidityInDays)
                };

                Response.Cookies.Append("jwt", request.Response!.Token!, cookieOptionsToken);
                Response.Cookies.Append("refreshToken", request.Response!.RefreshToken!, cookieOptionsRefreshToken);
                return Ok(new ResponseBase<UserInfoViewModel>
                {
                    Info = null,
                    Response = _mapper.Map<UserInfoViewModel>(request.Response)
                });
            }

            return BadRequest(new ResponseBase<UserInfoViewModel>
            {
                Info = request.Info,
                Response = null
            });
        }
    }
}
