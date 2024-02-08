using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Infrastructure.Interfaces;
using Accounts.Application.Builders;
using Accounts.Domain;
using Designly.Auth.Providers;
using Designly.Auth.Identity;
using LanguageExt.Common;
using Designly.Base.Exceptions;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, IAccountBuilder accountBuilder, IUnitOfWork unitOfWork, IIdentityService identityService) 
        : IRequestHandler<CreateAccountCommand, Result<Guid>>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAccountBuilder _accountBuilder = accountBuilder ?? throw new ArgumentNullException(nameof(accountBuilder));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
         
        public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Business Logic Pre-Validation
                var existingUser = await unitOfWork.UsersRepository.GetUserByEmailAsync(request.OwnerEmail, cancellationToken).ConfigureAwait(false);
                if (IsUserBlacklisted(existingUser))
                {
                    return new Result<Guid>(CreateBlacklistedUserException());
                }
                else if (UserAlreadyExists(existingUser))
                {
                    return new Result<Guid>(CreateExistingUserException());
                }

                var account = _accountBuilder.CreateBasicAccount(request.Name).Build();

                // save the account and get the id
                await unitOfWork.AccountsRepository.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                var accountOwner = new User(request.OwnerFirstName, request.OwnerLastName, request.OwnerEmail, request.OwnerJobTitle, account);

                // assign the account owner as the owner of the account
                account = _accountBuilder.ConfigureAccount(accountOwner).Build();

                // save changes and it's related entities
                await unitOfWork.AccountsRepository.UpdateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Register new user account at AWS
                await _identityService.CreateUserAsync(accountOwner.Email, accountOwner.FirstName, accountOwner.LastName, cancellationToken);

                // Create the tenant group at AWS
                string tenantGroup = $"{IdentityData.TenantIdClaimType + account.Id.ToString()}";
                await _identityService.CreateGroupAsync(tenantGroup, request.Name, cancellationToken);

                // Add the user to the tenant group at AWS
                await _identityService.AddUserToGroupAsync(accountOwner.Email, tenantGroup, cancellationToken);

                // Add the user to the account owners group at AWS
                await _identityService.AddUserToGroupAsync(accountOwner.Email, IdentityData.AccountOwnerGroup, cancellationToken);

                // Set the user password at AWS
                await _identityService.SetUserPasswordAsync(accountOwner.Email, request.OwnerPassword, cancellationToken);

                return account.Id;
            }
            catch (BusinessLogicException businessLogicException)
            {
                _logger.LogError(businessLogicException, "Could not create new account due to error: {exceptionType}: {exception.Message}", businessLogicException.GetType().Name, businessLogicException.Message);
                return new Result<Guid>(businessLogicException);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new account due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }

        private bool IsUserBlacklisted(User? existingUser) => existingUser is not null && existingUser.Status == Consts.UserStatus.Blacklisted;
        private bool UserAlreadyExists(User? existingUser) => existingUser is not null;

        private AccountException CreateBlacklistedUserException() => new AccountException(AccountErrors.UserEmailIsBlacklisted.Description, AccountErrors.UserEmailIsBlacklisted);
        private AccountException CreateExistingUserException() => new AccountException(AccountErrors.AccountAlreadyExists.Description, AccountErrors.AccountOwnerEmailAlreadyExists);
    }
}
