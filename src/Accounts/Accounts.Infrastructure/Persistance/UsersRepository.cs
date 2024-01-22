using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Clients.Infrastructure.Persistance;
using Clients.Infrastructure.Polly;
using Dapper;
using Designly.Shared.ConnectionProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Polly.Wrap;

namespace Accounts.Infrastructure.Persistance
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AccountsDbContext _context;
        private readonly ILogger<AccountsRepository> _logger;
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;
        private readonly AsyncPolicyWrap policy;

        public UsersRepository(AccountsDbContext context, ILogger<AccountsRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbConnectionStringProvider = dbConnectionStringProvider;

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Getting user by email: {email}", email);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Provided email is null or empty");
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _context.Users
                .Include( u => u.Account)
                .Include( u => u.Teams)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                .ConfigureAwait(false);

            return user;
        }
    }
}
