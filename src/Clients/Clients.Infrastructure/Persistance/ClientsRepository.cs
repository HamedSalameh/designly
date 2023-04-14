using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;

namespace Clients.Infrastructure.Persistance
{
    internal class ClientsRepository : IClientsRepository
    {
        public Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}