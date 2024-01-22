using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Accounts.Infrastructure.Persistance.Configuration;
using Clients.Infrastructure.Polly;
using Dapper;
using Designly.Shared.ConnectionProviders;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Account?> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            if (accountId == Guid.Empty || accountId == default)
            {
                _logger.LogError("Provided accountId is empty or default");
                throw new ArgumentNullException(nameof(accountId));
            }

            var account = await _context.Accounts
                .Include(a => a.Owner)
                .Include(a => a.Teams)
                .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

            return account;
        }

        public async Task<Guid> CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null)
            {
                _logger.LogError("Provided account entity is null");
                throw new ArgumentNullException(nameof(account));   
            }

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return account.Id;
        }

        public async Task UpdateAccount(Account account, CancellationToken cancellationToken)
        {
            if (account == null)
            {
                _logger.LogError("Provided account entity is null");
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Update(account);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> CreateUsersAsync(IEnumerable<User> users, CancellationToken cancellationToken)
        {
            if (users == null)
            {
                _logger.LogError("Provided users collection is null");
                throw new ArgumentNullException(nameof(users));
            }

            await _context.Users.AddRangeAsync(users, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return users;
        }

        public async Task<IEnumerable<Team>> UpdateTeamsAsync(IEnumerable<Team> teams, CancellationToken cancellationToken)
        {
            if (teams == null)
            {
                _logger.LogError("Provided teams collection is null");
                throw new ArgumentNullException(nameof(teams));
            }

            _context.Teams.UpdateRange(teams);

            await _context.SaveChangesAsync();

            return teams;
        }
    }
}
