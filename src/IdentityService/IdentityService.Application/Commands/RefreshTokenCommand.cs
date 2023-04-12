using IdentityService.Interfaces;
using MediatR;

namespace IdentityService.Application.Commands
{
    public class RefreshTokenCommand : IRequest<ITokenResponse?>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ITokenResponse?>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<ITokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = await _identityService.RefreshToken(request.RefreshToken, cancellationToken);

            return response;
        }
    }
}
