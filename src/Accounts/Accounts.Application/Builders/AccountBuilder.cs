
using Accounts.Domain;
using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Builders
{
    public class AccountBuilder : IAccountBuilder
    {
        private readonly ILogger<AccountBuilder> _logger;
        private Account? account;

        public AccountBuilder(ILogger<AccountBuilder> logger)
        {
            _logger = logger;
        }

        public IAccountBuilder CreateBasicAccount(string accountName)
        {
            // validate values
            if (string.IsNullOrWhiteSpace(accountName))
            {
                _logger.LogError($"Invalid value for {nameof(accountName)}");
                throw new ArgumentException($"The submitted account name is not valid or empty");
            }

            account = new Account(accountName);
            
            return this;
        }


        public Account Build()
        {
            if (account == null)
            {
                throw new BusinessLogicException("Account is not initialized");
            }

            return account;
        }

        public IAccountBuilder ConfigureAccount(User accountOwner)
        {
            if (account == default)
            {
                throw new BusinessLogicException("Account is not created yet");
            }

            account.AssignOwner(accountOwner);
            
            account.CreateDefaultTeam();
            
            account.AddUserToDefaultTeam(accountOwner);

            return this;
        }
    }

}
