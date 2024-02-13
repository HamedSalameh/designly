﻿using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Guid> CreateAccountAsync(Account account, CancellationToken cancellationToken);
        Task<Account?> GetAccountAsync(Guid accountId, CancellationToken cancellationToken);
        Task UpdateAccountAsync(Account account, CancellationToken cancellationToken);
    }
}
