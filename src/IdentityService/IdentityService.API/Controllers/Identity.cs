using Designly.Base.Exceptions;
using IdentityService.API.DTO;
using IdentityService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace IdentityService.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("createuser")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromForm] CreateUserRequestDetails createUserRequestDetails,
                       CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createUserCommand = new CreateUserCommand
            {
                Email = createUserRequestDetails.Email,
                FirstName = createUserRequestDetails.FirstName,
                LastName = createUserRequestDetails.LastName
            };

            _ = await _mediator.Send(createUserCommand, cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromForm] ClientSigningRequest clientSigningRequest,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var signinRequest = new SigninRequest(clientSigningRequest.Username, clientSigningRequest.Password);

            try
            {
                var tokenResponse = await _mediator.Send(signinRequest, cancellationToken);

                if (tokenResponse != null &&
                    !string.IsNullOrEmpty(tokenResponse.AccessToken) && !string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    // await SigninUserAsync(clientSigningRequest);
                }

                return Ok(tokenResponse);
            }
            catch (BusinessLogicException businessLogicException)
            {
                var problemDetails = businessLogicException.ToDesignlyProblemDetails(statusCode: System.Net.HttpStatusCode.BadRequest);
                return BadRequest(problemDetails);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not sign in user due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("signout")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Signout(CancellationToken cancellation)
        {
            var accessToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);

            var signoutRequest = new SignoutRequest(accessToken);

            await _mediator.Send(signoutRequest, cancellation).ConfigureAwait(false);

            // await UserSignoutAsync();

            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellation)
        {
            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
            {
                return BadRequest();
            }
            var refreshToken = refreshTokenRequest.RefreshToken;
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(nameof(refreshToken));
            }

            var refreshTokenCommand = new RefreshTokenCommand(refreshToken);

            var response = await _mediator.Send(refreshTokenCommand, cancellation).ConfigureAwait(false);

            return Ok(response);
        }

        /// <summary>
        /// Reference at https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0
        /// </summary>
        /// <param name="clientSigningRequest"></param>
        /// <returns></returns>
        private async Task SigninUserAsync(ClientSigningRequest clientSigningRequest)
        {
            var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, clientSigningRequest.Username)
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        /// <summary>
        /// Reference at https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0
        /// </summary>
        /// <param name="clientSigningRequest"></param>
        /// <returns></returns>
        private async Task UserSignoutAsync()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
