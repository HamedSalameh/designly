namespace Accounts.Infrastructure.Interfaces
{

    public interface IUnitOfWork
    {
        IAccountsRepository AccountsRepository { get; }
    }
}
