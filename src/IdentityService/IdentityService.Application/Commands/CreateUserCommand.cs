using Designly.Shared;
using IdentityService.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(Consts.FirstNameMaxLength, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(Consts.LastNameMaxLength, MinimumLength = 2)]
        public required string LastName { get; set; }
    }

    public class CreateUserCommandHandler(IIdentityService identityService, ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        private readonly ILogger<CreateUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Handling request {nameof(CreateUserCommandHandler)} for {nameof(request.Email)} {request.Email}");
            }

            var response = await _identityService.CreateUser(request.Email, request.FirstName, request.LastName, cancellationToken);
            return response;

        }
    }
}