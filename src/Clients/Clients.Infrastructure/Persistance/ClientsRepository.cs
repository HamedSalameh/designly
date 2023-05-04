using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Clients.Infrastructure.Polly;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Clients.Infrastructure.Persistance
{
    internal class ClientsRepository : IClientsRepository
    {
        private readonly ClientsDBContext _dbContext;
        private readonly ILogger<ClientsRepository> _logger;

        public ClientsRepository(ClientsDBContext dbContext, ILogger<ClientsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var policy = PollyPolicyFactory.WrappedAsyncPolicies();
            
            await _dbContext.Clients.AddAsync(client, cancellationToken).ConfigureAwait(false);

            _ = policy.ExecuteAsync(async () => await _dbContext.SaveChangesAsync().ConfigureAwait(false));
            
            _logger.LogDebug($"Client {client.Id} was successfully created.");

            return client.Id;
        }

        public async Task<Client?> GetClientAsyncNoTracking(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var client = await _dbContext.FindAsync<Client>(id, cancellationToken).ConfigureAwait(false);

            return client;
        }
    }
}