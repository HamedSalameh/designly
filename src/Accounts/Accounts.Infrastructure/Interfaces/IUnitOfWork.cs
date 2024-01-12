namespace Clients.Infrastructure.Interfaces
{

    public interface IUnitOfWork
    {
        IAccountsRepository AccountsRepository { get; }
    }
}
