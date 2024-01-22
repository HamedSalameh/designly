using Accounts.Infrastructure.Interfaces;

namespace Accounts.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountsRepository AccountsRepository { get; }
        public IUsersRepository UsersRepository { get; }

        public UnitOfWork(IAccountsRepository accountsRepository, IUsersRepository usersRepository)
        {
            AccountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }
    }
}