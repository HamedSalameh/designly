namespace Accounts.Infrastructure.Interfaces
{

    public interface IUnitOfWork
    {
        IAccountsRepository AccountsRepository { get; }
        IUsersRepository UsersRepository { get; }
    }
}
