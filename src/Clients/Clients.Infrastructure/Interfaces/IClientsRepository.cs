using Clients.Domain.Entities;

namespace Clients.Infrastructure.Interfaces
{
    public interface IClientsRepository
    {
        Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken);
        Task<Client?> GetClientAsyncNoTracking(Guid id, CancellationToken cancellationToken);
    }
}
