using Flow.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public class SignoutRequest : IRequest<bool>
    {
        public string AccessToken { get; init; }

        public SignoutRequest(string accessToken)
        {
            AccessToken = accessToken;
        }
    }

    public class SignoutRequestHandler : IRequestHandler<SignoutRequest, bool>
    {
        private readonly ILogger<SignoutRequestHandler> _logger;
        private readonly IIdentityService _identityService;

        public SignoutRequestHandler(ILogger<SignoutRequestHandler> logger, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<bool> Handle(SignoutRequest request, CancellationToken cancellationToken)
        {
            var accessToken = request?.AccessToken ?? string.Empty;

            return await _identityService.SignoutAsync(accessToken, cancellationToken);
        }
    }
}
