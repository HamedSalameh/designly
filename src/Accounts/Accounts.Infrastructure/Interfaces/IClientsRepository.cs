

using Accounts.Domain;

namespace Clients.Infrastructure.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Guid> CreateAccountAsync(Account client, CancellationToken cancellationToken);
    }
}
