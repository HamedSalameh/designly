using Clients.Domain.Entities;

namespace Clients.Infrastructure.Interfaces
{
    public interface IClientsRepository
    {
        Task<Client?> GetClientAsyncWithDapper(Guid tenantId, Guid clientId, CancellationToken cancellationToken);
        Task DeleteClientAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken);
        Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken);
        Task<IEnumerable<Client>> SearchClientsAsync(Guid tenantId, string firstName, string familyName, string city, CancellationToken cancellationToken);
        Task<Guid> CreateClientAsyncWithDapper(Client client, CancellationToken cancellationToken);
    }
}
