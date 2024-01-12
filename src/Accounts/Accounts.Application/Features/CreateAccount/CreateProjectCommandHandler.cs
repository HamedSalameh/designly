using Projects.Application.Features.CreateAccount;
using Designly.Auth.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using Clients.Infrastructure.Interfaces;
using Accounts.Domain;

namespace Projects.Application.Features.CreateProject
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger;
        private readonly IUnitOfWork unitOfWork;

        public CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = new Account(request.Name, request.AccountOwner);
                var accountId = await unitOfWork.AccountsRepository.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                return accountId;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }
    }
}
