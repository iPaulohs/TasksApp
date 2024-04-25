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

        /// <summary>
        /// Rota responsável pela criação de um usuário.
        /// </summary>
        /// <param name="command">
        /// Um objeto CreateUserCommand
        /// </param>
        /// <returns>Os dados do usuário criado.</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /Auth/Create-User
        /// {
        ///    "name": "Johh",
        ///     "surname": "Doe",
        ///     "username": "JDoe",
        ///     "email": "jdoe@mail.com",
        ///     "password": "123456"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados de um novo usuário</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
        [HttpPost("Create-User")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>?>> CreateUser([FromBody] CreateUserCommand command)
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
                    return Ok(request);
                }
            }

            return BadRequest(request);
        }

        /// <summary>
        /// Rota responsável pelo login do usuário.
        /// </summary>
        /// <param name="command">
        /// Um objeto LoginUserCommand
        /// </param>
        /// <returns>Os dados do usuário logado.</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /Auth/Create-User
        /// {
        ///     "email": "jdoe@mail.com",
        ///     "password": "123456"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados do usuário se o login tiver ocorrido corretamente</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
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
                    return Ok(request);
                }
            }

            return BadRequest(request);
        }


        /// <summary>
        /// Rota responsável pelo refresh token do usuário
        /// </summary>
        /// <param name="command">
        /// Um objeto LoginUserCommand
        /// </param>
        /// <returns>Os dados do usuário logado.</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /Auth/Create-User
        /// {
        ///     "username": "JDoe",
        ///     "RefreshToken": "refreshtoken"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados do usuário se o login tiver ocorrido corretamente</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
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
                return Ok(request);
            }

            return BadRequest(request);
        }
    }
}
