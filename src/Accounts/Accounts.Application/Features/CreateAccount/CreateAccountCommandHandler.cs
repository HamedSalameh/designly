﻿using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Infrastructure.Interfaces;
using Accounts.Application.Builders;
using Accounts.Domain;
using Designly.Auth.Providers;
using Designly.Auth.Identity;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, IAccountBuilder accountBuilder, IUnitOfWork unitOfWork, IIdentityService identityService) : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAccountBuilder _accountBuilder = accountBuilder ?? throw new ArgumentNullException(nameof(accountBuilder));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = _accountBuilder.CreateBasicAccount(request.Name).Build();

                var accountOwner = new User(request.OwnerFirstName, request.OwnerLastName, request.OwnerEmail, request.OwnerJobTitle, account);

                // assign the account owner as the owner of the account
                account = _accountBuilder.ConfigureAccount(accountOwner).Build();

                // save changes
                await unitOfWork.AccountsRepository.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Register new user account at AWS
                await _identityService.CreateUser(accountOwner.Email, accountOwner.FirstName, accountOwner.LastName, cancellationToken);

                // Create the tenant group at AWS
                string tenantGroup = $"{IdentityData.TenantIdClaimType + account.Id.ToString()}";
                await _identityService.CreateGroup(tenantGroup, request.Name, cancellationToken);

                // Add the user to the tenant group at AWS
                await _identityService.AddUserToGroup(accountOwner.Email, tenantGroup, cancellationToken);

                // Set the user password at AWS
                await _identityService.SetUserPasswordAsync(accountOwner.Email, request.OwnerPassword, cancellationToken);

                return account.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }
    }
}
