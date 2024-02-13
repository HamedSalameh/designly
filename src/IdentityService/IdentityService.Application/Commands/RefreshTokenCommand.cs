using Designly.Auth.Models;
using Designly.Auth.Providers;
using MediatR;

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

    public class RefreshTokenCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, ITokenResponse?>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<ITokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = await _identityService.RefreshToken(request.RefreshToken, cancellationToken);

            return response;
        }
    }
}
