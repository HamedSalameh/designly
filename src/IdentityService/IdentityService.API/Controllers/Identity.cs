using Asp.Versioning;
using Designly.Auth.Identity;
using Designly.Auth.Models;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using IdentityService.API.DTO;
using IdentityService.Application.Commands;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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

        public IdentityController(IMediator mediator, ILogger<IdentityController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        [Authorize]
        [HttpGet("is-authenticated")]
        public IActionResult IsAuthenticated()
        {
            var user = HttpContext?.User;
            if (user is null || (user.Identity?.IsAuthenticated is false))
            {
                return Unauthorized();
            }

            var IdToken = HttpContext?.Request.Cookies["id_token"];
            // parse the id token from base64
            var parsedToken = new JwtSecurityToken(IdToken);

            return Ok(CreateAuthenticatedUser(parsedToken.Claims));
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromForm] ClientSigningRequest clientSigningRequest, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var signinRequest = new SigninRequest(clientSigningRequest.Username, clientSigningRequest.Password);

            try
            {
                var tokenResponse = await _mediator.Send(signinRequest, cancellationToken);
                return parseTokenResponseResult(tokenResponse);
            }
            catch (BusinessLogicException businessLogicException)
            {
                _logger.LogError(businessLogicException, "Could not sign in user due to error: {Message}", businessLogicException.Message);
                var problemDetails = businessLogicException.ToDesignlyProblemDetails(statusCode: System.Net.HttpStatusCode.BadRequest);
                return BadRequest(problemDetails);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogWarning(operationCanceledException, "Signin request was cancelled: {Message}", operationCanceledException.Message);
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not sign in user due to error: {ExceptionType}: {Message}", exception.GetType().Name, exception.Message);
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

            // remove the token from the cookies
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

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

            // using the new token from refreshtoken, set the cookie
            if (response is not null && response is ITokenResponse tokenResponse)
            {
                SetCookie("access_token", tokenResponse.AccessToken, 10);
            }

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

        private AuthenticatedUser CreateAuthenticatedUser(IEnumerable<Claim> claims)
        {
            var tenantId = Guid.Empty;
            var tenantClaim = claims.FirstOrDefault(claim => claim.Type == "cognito:groups" && claim.Value.StartsWith(IdentityData.TenantIdClaimType))?.Value;
            if (!string.IsNullOrEmpty(tenantClaim))
            {
                Guid.TryParse(tenantClaim.Split('_')[1], out tenantId);
            }

            var givenName = claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var familyName = claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
            var email = claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;

            var roles = claims.Where(c => c.Type == "cognito:groups").Select(c => c.Value).ToArray();
            var permissions = claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToArray();
            var profileImage = claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            return new AuthenticatedUser(givenName, familyName, email, tenantId, roles, permissions, profileImage);
        }

        private AuthenticatedUser CreateAuthenticatedUser(string idToken)
        {
            var jwtToken = new JwtSecurityToken(idToken);
            var claims = jwtToken.Claims;

            return CreateAuthenticatedUser(claims);
        }

        private IActionResult parseTokenResponseResult(Result<ITokenResponse> tokenResponse)
        {
            return tokenResponse.Match(
                Succ: tokenResponse =>
                {
                    if (tokenResponse != null &&
                        !string.IsNullOrEmpty(tokenResponse.AccessToken) && !string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    {
                        // await SigninUserAsync(clientSigningRequest);
                        SetCookie("access_token", tokenResponse.AccessToken, 10);
                        SetCookie("refresh_token", tokenResponse.RefreshToken, 60);
                        SetCookie("id_token", tokenResponse.IdToken, 10);

                        // create the authenticated user respose
                        var user = CreateAuthenticatedUser(tokenResponse.IdToken);
                        return Ok(user); // Return IActionResult here.
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError);
                },
                Fail: exception =>
                {
                    _logger.LogError(exception, "Signin request failed: {Message}", exception.Message);
                    return exception switch
                    {
                        BusinessLogicException businessLogicException => GetResponse(businessLogicException),
                        _ => BadRequest(exception.Message) // Use BadRequest to return IActionResult.
                    };
                });
        }

        private IActionResult GetResponse(BusinessLogicException businessLogicException)
        {
            var error = businessLogicException.DomainErrors?.FirstOrDefault() ?? new KeyValuePair<string, string>("Error", businessLogicException.Message);

            return error.Key switch
            {
                "InvalidCredentials" => Unauthorized(error.Value),
                _ => BadRequest(businessLogicException.ToDesignlyProblemDetails(statusCode: System.Net.HttpStatusCode.BadRequest))
            };
        }

        private void SetCookie(string name, string value, int minutes)
        {
            Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            });
        }

    }
}
