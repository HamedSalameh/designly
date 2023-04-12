using IdentityService.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class SigninRequest : IRequest<ITokenResponse>
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public SigninRequest(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }

    public class SigninRequestHandler : IRequestHandler<SigninRequest, ITokenResponse?>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<SigninRequestHandler> logger;

        public SigninRequestHandler(IIdentityService identityService, ILogger<SigninRequestHandler> logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ITokenResponse?> Handle(SigninRequest request, CancellationToken cancellationToken)
        {
            var username = request.Username;
            var password = request.Password;

            logger.LogDebug($"Handling request {nameof(SigninRequestHandler)} for {nameof(username)} {username}");
            var tokenResponse = await _identityService.LoginAsync(username, password, cancellationToken);

            return tokenResponse;
        }
    }
}
