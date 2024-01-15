using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Clients.Infrastructure.Persistance;
using Clients.Infrastructure.Polly;
using Dapper;
using Designly.Shared.ConnectionProviders;

using Microsoft.Extensions.Logging;

using Polly.Wrap;

namespace Accounts.Infrastructure.Persistance
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly AccountsDbContext _context;
        private readonly ILogger<AccountsRepository> _logger;
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;
        private readonly AsyncPolicyWrap policy;

        public AccountsRepository(ILogger<AccountsRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider, AccountsDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbConnectionStringProvider = dbConnectionStringProvider;

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            _context = context;
        }

        public async Task<Guid> CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null)
            {
                _logger.LogError("Provided account entity is null");
                throw new ArgumentNullException(nameof(account));   
            }

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return account.Id;
        }
    }
}
