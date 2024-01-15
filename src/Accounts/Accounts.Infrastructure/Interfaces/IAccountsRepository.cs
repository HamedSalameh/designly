using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Guid> CreateAccountAsync(Account client, CancellationToken cancellationToken);
    }
}
