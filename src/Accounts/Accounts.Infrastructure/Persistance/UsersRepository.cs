﻿using Accounts.Domain;
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
                _logger.LogDebug($"Getting user by email for {email}");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Provided email is null or empty");
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _context.Users
                .Include(u => u.Account)
                .Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                .ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Got user by email for {email} : {user?.ToString()}");
            }

            return user;
        }

        public async Task<User?> GetTenantUserByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Getting tenant user by email for {email} in tenant {tenantId}");
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
                _logger.LogDebug($"Got tenant user by email for {email} in tenant {tenantId} : {user?.ToString()}");
            }

            return user;
        }
    }
}
