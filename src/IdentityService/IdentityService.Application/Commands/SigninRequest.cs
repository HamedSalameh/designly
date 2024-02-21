using Designly.Auth.Models;
using Designly.Auth.Providers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class SigninRequest(string Username, string Password) : IRequest<ITokenResponse>
    {
        public string Username { get; init; } = Username;
        public string Password { get; init; } = Password;
    }

    public class SigninRequestHandler(IIdentityService identityService, ILogger<SigninRequestHandler> logger) : IRequestHandler<SigninRequest, ITokenResponse?>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        private readonly ILogger<SigninRequestHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<ITokenResponse?> Handle(SigninRequest request, CancellationToken cancellationToken)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Handling request {SigninRequestHandler} for {username}", nameof(SigninRequestHandler), request.Username);
            }

            var username = request.Username;
            var password = request.Password;

            logger.LogDebug("Handling request {SigninRequestHandler} for {username}", nameof(SigninRequestHandler), username);
            var tokenResponse = await _identityService.LoginJwtAsync(username, password, cancellationToken);

            return tokenResponse;
        }
    }
}
