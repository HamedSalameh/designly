using Clients.Infrastructure.Interfaces;

namespace Clients.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IClientsRepository ClientsRepository { get; }

        public UnitOfWork(IClientsRepository clientsRepository)
        {
            ClientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));
        }
    }
}