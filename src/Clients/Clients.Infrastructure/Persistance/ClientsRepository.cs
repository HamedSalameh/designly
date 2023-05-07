using Clients.Domain;
using Clients.Domain.Entities;
using Clients.Domain.ValueObjects;
using Clients.Infrastructure.Interfaces;
using Clients.Infrastructure.Polly;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using SharedKernel.ConnectionProviders;

namespace Clients.Infrastructure.Persistance
{
    internal class ClientsRepository : IClientsRepository
    {
        private readonly ClientsDBContext _dbContext;
        private readonly ILogger<ClientsRepository> _logger;
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;
        private readonly AsyncPolicyWrap policy;

        public ClientsRepository(ClientsDBContext dbContext, ILogger<ClientsRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbConnectionStringProvider = dbConnectionStringProvider;

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
        }

        public async Task<Guid> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var policy = PollyPolicyFactory.WrappedAsyncPolicies();

            await _dbContext.Clients.AddAsync(client, cancellationToken).ConfigureAwait(false);

            _ = policy.ExecuteAsync(async () => await _dbContext.SaveChangesAsync().ConfigureAwait(false));

            _logger.LogDebug("Client {client.Id} was successfully created.", client.Id);

            return client.Id;
        }

        public async Task DeleteClientAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var policy = PollyPolicyFactory.WrappedAsyncPolicies();
            var entity = await _dbContext.Clients.FindAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(id.ToString());
            }

            _dbContext.Remove(entity);
            _ = policy.ExecuteAsync(async () => await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false));
            _logger.LogDebug("Delete client: {id}", id);
        }

        public async Task<Client?> GetClientAsyncNoTracking(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var client = await _dbContext.Clients.AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken)
                .ConfigureAwait(false);

            return client;
        }

        public async Task<Client?> GetClientAsyncWithDapper(Guid id, CancellationToken cancellationToken)
        {
            var sqlCommand = "SELECT * FROM clients WHERE id=@id";

            var dynamic = new DynamicParameters();
            dynamic.Add(nameof(id), id);

            _logger.LogDebug("{sqlCommand} : {sqlParameters}", sqlCommand, dynamic);
            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync();

                var client = await policy.ExecuteAsync(async () =>
                {
                    var result = await connection.QueryAsync<Client, Address, ContactDetails, Client>(
                    new CommandDefinition(sqlCommand, parameters: dynamic, cancellationToken: cancellationToken),
                    (client, address, contactDetails) =>
                    {
                        client.Address = address;
                        client.ContactDetails = contactDetails;
                        return client;
                    }, "city, primary_phone_number")        // since we are using value objects from different class, we need to set the split/join on field.
                    .ConfigureAwait(false);

                    return result.FirstOrDefault();
                });

                return client;
            };
        }
    }
}