using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Infrastructure.Interfaces;
using Accounts.Application.Builders;
using Accounts.Domain;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, IAccountBuilder accountBuilder, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAccountBuilder _accountBuilder = accountBuilder ?? throw new ArgumentNullException(nameof(accountBuilder));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = _accountBuilder.CreateBasicAccount(request.Name).Build();

                var accountId = await unitOfWork.AccountsRepository.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                var accountOwner = new User(request.OwnerFirstName, request.OwnerLastName, request.OwnerEmail, request.OwnerJobTitle, account);

                // assign the account owner as the owner of the account
                account = _accountBuilder.ConfigureAccount(accountOwner).Build();

                // save changes
                var completeAccount = await unitOfWork.AccountsRepository.SaveChanges(account, cancellationToken);

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
