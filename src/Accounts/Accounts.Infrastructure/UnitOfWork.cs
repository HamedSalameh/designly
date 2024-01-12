using Clients.Infrastructure.Interfaces;

namespace Clients.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountsRepository AccountsRepository { get; }

        public UnitOfWork(IAccountsRepository accountsRepository)
        {
            AccountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
        }
    }
}