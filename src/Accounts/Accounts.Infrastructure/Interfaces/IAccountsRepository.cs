using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Guid> CreateAccountAsync(Account client, CancellationToken cancellationToken);
        Task<Account?> GetAccountAsync(Guid accountId, CancellationToken cancellationToken);
        Task UpdateAccount(Account account, CancellationToken cancellationToken);

        Task<IEnumerable<Team>> UpdateTeamsAsync(IEnumerable<Team> teams, CancellationToken cancellationToken);

        Task<IEnumerable<User>> CreateUsersAsync(IEnumerable<User> users, CancellationToken cancellationToken);
    }
}
