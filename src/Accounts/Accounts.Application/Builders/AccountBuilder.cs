
using Accounts.Domain;
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
            // TODO: Add validation
            if (account == null)
            {
                throw new Exception("Account is not initialized");
            }

            return account;
        }

        public IAccountBuilder ConfigureAccount(User accountOwner)
        {
            if (account == default || account is null)
            {
                throw new Exception("Account is not created yet");
            }

            account.AssignOwner(accountOwner);
            
            account.CreateDefaultTeam(accountOwner);
            
            account.AddUserToDefaultTeam(accountOwner);

            return this;
        }
    }

}
