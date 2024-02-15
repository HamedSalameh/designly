using Designly.Auth.Providers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class SignoutRequest(string accessToken) : IRequest<Unit>
    {
        public string AccessToken { get; init; } = accessToken;
    }

    public class SignoutRequestHandler(ILogger<SignoutRequestHandler> logger, IIdentityService identityService) : IRequestHandler<SignoutRequest, Unit>
    {
        private readonly ILogger<SignoutRequestHandler> _logger = logger;
        private readonly IIdentityService _identityService = identityService;

        public async Task<Unit> Handle(SignoutRequest request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SignoutRequestHandler} for {request.AccessToken}", nameof(SignoutRequestHandler), request.AccessToken);
            }
            var accessToken = request?.AccessToken ?? string.Empty;

            await _identityService.SignoutAsync(accessToken, cancellationToken);

            return Unit.Value;
        }
    }
}
