using Designly.Auth.Models;
using Designly.Auth.Providers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class RefreshTokenCommand : IRequest<ITokenResponse?>
    {
        public string RefreshToken { get; set; }

        public RefreshTokenCommand(string refreshToken)
        {
            ArgumentNullException.ThrowIfNull(refreshToken);
            RefreshToken = refreshToken;
        }
    }

    public class RefreshTokenCommandHandler(IIdentityService identityService, ILogger<RefreshTokenCommandHandler> logger) : IRequestHandler<RefreshTokenCommand, ITokenResponse?>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<ITokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {RefreshTokenCommandHandler} for {RefreshToken}", nameof(RefreshTokenCommandHandler), request.RefreshToken);
            }

            var response = await _identityService.RefreshToken(request.RefreshToken, cancellationToken);

            return response;
        }
    }
}
