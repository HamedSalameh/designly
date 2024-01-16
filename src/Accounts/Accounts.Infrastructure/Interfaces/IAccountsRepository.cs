using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Guid> CreateAccountAsync(Account client, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<Account> SaveChanges(Account account, CancellationToken cancellationToken);
    }
}
