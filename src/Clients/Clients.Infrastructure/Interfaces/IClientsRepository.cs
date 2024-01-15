using Clients.Domain.Entities;

namespace Clients.Infrastructure.Interfaces
{
    public interface IClientsRepository
    {
        Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken);
        Task<Client?> GetClientAsyncWithDapper(Guid TenantId, Guid id, CancellationToken cancellation);
        Task DeleteClientAsync(Guid TenantId, Guid id, CancellationToken cancellationToken);
        Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken);
        Task<IEnumerable<Client>> SearchClientsAsync(Guid tenantId, string firstName, string familyName, string city, CancellationToken cancellationToken);
        Task<Guid> CreateClientAsyncWithDapper(Client client, CancellationToken cancellationToken);
    }
}
