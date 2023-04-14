namespace Clients.Infrastructure.Interfaces
{

    public interface IUnitOfWork
    {
        IClientsRepository ClientsRepository { get; }
    }
}
