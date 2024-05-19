using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Accounts.Infrastructure.Persistance.Configuration;
using Dapper;
using Designly.Shared.Polly;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly.Wrap;
using static Accounts.Domain.Consts;

namespace Accounts.Infrastructure.Persistance
{
    public sealed class UsersRepository : IUsersRepository
    {
        private readonly AccountsDbContext _context;
        private readonly ILogger<UsersRepository> _logger;
        private readonly AsyncPolicyWrap _policy;

        public UsersRepository(AccountsDbContext context, ILogger<UsersRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(logger);

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            _policy = PollyPolicyFactory.WrappedAsyncPolicies();
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Getting user by email for {Email}", email);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Provided email is null or empty");
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _policy.ExecuteAsync(async () =>
            {
                var user = await _context.Users
                    .Include(u => u.Account)
                    .Include(u => u.Teams)
                    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                    .ConfigureAwait(false);
                
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Got user by email for {Email} : {User}", email, user);
                }

                return user;
            });

            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Getting user by userId for {UserId}", userId);
            }
            
            var user = await _policy.ExecuteAsync(async () =>
            {
                var user = await _context.Users
                    .Include(u => u.Account)
                    .Include(u => u.Teams)
                    .FirstOrDefaultAsync(u => u.Id == userId && u.AccountId == tenantId, cancellationToken)
                    .ConfigureAwait(false);

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Got user by userId for {UserId} : {User}", userId, user);
                }

                return user;
            });

            return user;
        }

        public async Task<UserStatus?> GetUserStatusAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Getting user status by userId for {UserId}", userId);
            }

            var userStatus = await _policy.ExecuteAsync(async () =>
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId && u.AccountId == tenantId, cancellationToken)
                    .ConfigureAwait(false);

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Got user status by userId for {UserId} : {User}", userId, user);
                }

                return user?.Status;
            });

            return userStatus;
        }

        public async Task<User?> GetTenantUserByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Getting tenant user by email for {Email} in tenant {TenantId}", email, tenantId);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Provided email is null or empty");
                throw new ArgumentNullException(nameof(email));
            }

            if (tenantId == Guid.Empty)
            {
                _logger.LogError("Provided tenantId is empty");
                throw new ArgumentNullException(nameof(tenantId));
            }

            var user = await _context.Users
                .Include(u => u.Account)
                .Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.Email == email && u.Account.Id == tenantId, cancellationToken)
                .ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Got tenant user by email for {Email} in tenant {TenantId} : {User}", email, tenantId, user);
            }

            return user;
        }
    }
}
