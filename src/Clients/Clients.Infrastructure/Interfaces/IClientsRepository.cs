using Clients.Domain.Entities;
using System.Diagnostics.Eventing.Reader;

namespace Clients.Infrastructure.Interfaces
{
    public interface IClientsRepository
    {
        Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken);
        Task<Client?> GetClientAsyncNoTracking(Guid id, CancellationToken cancellationToken);
        Task<Client?> GetClientAsyncWithDapper(Guid id, CancellationToken cancellation);
        Task DeleteClientAsync(Guid id, CancellationToken cancellationToken);
    }
}
