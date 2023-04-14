using Clients.Domain.Entities;

namespace Clients.Infrastructure.Interfaces
{
    public interface IClientsRepository
    {
        Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken);
    }
}
