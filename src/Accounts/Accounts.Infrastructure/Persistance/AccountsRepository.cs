﻿using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Accounts.Infrastructure.Persistance.Configuration;
using Clients.Infrastructure.Polly;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Polly.Wrap;

namespace Accounts.Infrastructure.Persistance
{
    public sealed class AccountsRepository : IAccountsRepository
    {
        private readonly AccountsDbContext _context;
        private readonly ILogger<AccountsRepository> _logger;
        private readonly AsyncPolicyWrap policy;

        public AccountsRepository(ILogger<AccountsRepository> logger, AccountsDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            _context = context;
        }

        public async Task<Account?> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            if (accountId == Guid.Empty)
            {
                _logger.LogError("Provided accountId is empty or default");
                throw new ArgumentNullException(nameof(accountId));
            }

            // wrap in polly policy

            await policy.ExecuteAsync(async () =>
            {
                var account = await _context.Accounts
                    .Include(a => a.Owner)
                    .Include(a => a.Teams)
                    .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

                return account;
            });

            return null;
        }

        public async Task<Guid> CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null)
            {
                _logger.LogError("Provided account entity is null");
                throw new ArgumentNullException(nameof(account));   
            }

            await _context.Accounts.AddAsync(account, cancellationToken);

            // wrap in polly policy
            await policy.ExecuteAsync(async () =>
            {
                await _context.SaveChangesAsync(cancellationToken);
            });

            return account.Id;
        }

        public async Task UpdateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null)
            {
                _logger.LogError("Provided account entity is null");
                throw new ArgumentNullException(nameof(account));
            }

            // open new transaction
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // wrap in polly policy
                await policy.ExecuteAsync(async () =>
                {
                    // update the account and any related entities
                    _context.Accounts.Update(account);

                    await _context.SaveChangesAsync();
                });

                transaction.Commit();
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(exception, "Could not update account due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }
    }
}
