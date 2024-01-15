using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

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
