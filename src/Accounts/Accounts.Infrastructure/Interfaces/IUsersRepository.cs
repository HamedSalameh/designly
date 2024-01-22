using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken);
    }
}
