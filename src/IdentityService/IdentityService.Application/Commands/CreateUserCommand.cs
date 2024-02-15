using Designly.Auth.Providers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }
    }

    public class CreateUserCommandHandler(IIdentityService identityService, ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        private readonly ILogger<CreateUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateUserCommandHandler} for {request.Email}, {request.FirstName}, {request.LastName}", 
                                                       nameof(CreateUserCommandHandler), request.Email, request.FirstName, request.LastName);
            }

            var response = await _identityService.CreateUserAsync(request.Email, request.FirstName, request.LastName, cancellationToken);
            return response;

        }
    }
}