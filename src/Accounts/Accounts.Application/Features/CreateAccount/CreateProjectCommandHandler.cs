using Accounts.Application.Features.CreateAccount;
using Designly.Auth.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Projects.Application.Features.CreateProject
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger;

        public CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.NewGuid();

                // TODO: Create account in the database

                return await Task.FromResult(accountId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }
    }
}
