using Amazon.CognitoIdentityProvider.Model;
using Clients.API.DTO;
using Clients.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;

namespace Clients.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class Identity : ControllerBase
    {
        private readonly IMediator _mediator;

        public Identity(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("/signin")]
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

            var tokenResponse = await _mediator.Send(signinRequest, cancellationToken);

            if (tokenResponse != null &&
                !string.IsNullOrEmpty(tokenResponse.AccessToken) && !string.IsNullOrEmpty(tokenResponse.RefreshToken))
            {
                await userSigningAsync(clientSigningRequest);
            }

            return Ok(tokenResponse);
        }

        [HttpPost]
        [Route("/signout")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Signout(CancellationToken cancellation)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", string.Empty);

            var signoutRequest = new SignoutRequest(accessToken);

            _ = await _mediator.Send(signoutRequest, cancellation).ConfigureAwait(false);
            
            await userSignoutAsync();

            return Ok();
        }

        [HttpPost]
        [Route("/Refresh")]
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

            var refreshTokenCommand = new RefreshTokenCommand()
            {
                RefreshToken = refreshTokenRequest.RefreshToken
            };

            var response = await _mediator.Send(refreshTokenCommand, cancellation).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("/Test")]
        public IActionResult TestAuthentication()
        {
            // TODO

            return Ok($"This is secured!");
        }

        /// <summary>
        /// Reference at https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0
        /// </summary>
        /// <param name="clientSigningRequest"></param>
        /// <returns></returns>
        private async Task userSigningAsync(ClientSigningRequest clientSigningRequest)
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, clientSigningRequest.Username)
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
        private async Task userSignoutAsync()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
