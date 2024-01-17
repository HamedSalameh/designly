using IdentityService.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class SignoutRequest : IRequest<Unit>
    {
        public string AccessToken { get; init; }

        public SignoutRequest(string accessToken)
        {
            AccessToken = accessToken;
        }
    }

    public class SignoutRequestHandler : IRequestHandler<SignoutRequest, Unit>
    {
        private readonly ILogger<SignoutRequestHandler> _logger;
        private readonly IIdentityService _identityService;

        public SignoutRequestHandler(ILogger<SignoutRequestHandler> logger, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(SignoutRequest request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Handling request {nameof(SignoutRequestHandler)} for {nameof(request.AccessToken)} {request.AccessToken}");
            }
            var accessToken = request?.AccessToken ?? string.Empty;

            await _identityService.SignoutAsync(accessToken, cancellationToken);

            return Unit.Value;
        }
    }
}
