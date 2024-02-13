using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Clients.Infrastructure.Polly;
using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using Polly.Wrap;
using SqlKata;
using SqlKata.Compilers;
using System.Data;

namespace Clients.Infrastructure.Persistance
{
    internal sealed class ClientsRepository : IClientsRepository
    {
        private readonly ILogger<ClientsRepository> _logger;
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;
        private readonly AsyncPolicyWrap policy;

        public ClientsRepository(ILogger<ClientsRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(dbConnectionStringProvider);
            
            _logger = logger;
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            this.dbConnectionStringProvider = dbConnectionStringProvider;

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
        }

        // Create a new client with Dapper
        public async Task<Guid> CreateClientAsyncWithDapper(Client client, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var parameters = new DynamicParameters();
            parameters.Add("p_tenant_id", client.TenantId, DbType.Guid);
            parameters.Add("p_first_name", client.FirstName, DbType.String);
            parameters.Add("p_family_name", client.FamilyName, DbType.String);
            parameters.Add("p_status", client.Status, DbType.Int16);
            parameters.Add("p_city", client.Address.City, DbType.String);
            parameters.Add("p_street", client.Address.Street, DbType.String);
            parameters.Add("p_building_number", client.Address.BuildingNumber, DbType.String);
            parameters.Add("p_address_lines", JsonConvert.SerializeObject(client.Address.AddressLines));
            parameters.Add("p_primary_phone_number", client.ContactDetails.PrimaryPhoneNumber, DbType.String);
            parameters.Add("p_secondary_phone_number", client.ContactDetails.SecondaryPhoneNumber, DbType.String);
            parameters.Add("p_email_address", client.ContactDetails.EmailAddress, DbType.String);
            parameters.Add("p_client_id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync("create_client", parameters,
                                               transaction: transaction, commandType: CommandType.StoredProcedure);

                    // Retrieve the returned ID from the output parameter
                    var insertedId = parameters.Get<Guid>("p_client_id");
                    client.Id = insertedId;

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Could not create client entity due to error : {exception.Message}");
                    transaction.Rollback();
                    throw;
                }
            }

            return client.Id;
        }

        public async Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            if (client == default || client == null)
            {
                _logger.LogError($"Invalid value for {nameof(client)}: {client}");
                throw new ArgumentException($"Invalid value of client object");
            }
            if (client.Id == default)
            {
                _logger.LogError($"Invalid value for {nameof(client.Id)}: {client.Id}");
                throw new ArgumentException("Client object has invalid value for Id property.");
            }

            var parameters = new DynamicParameters();
            parameters.Add("p_id", client.Id, DbType.Guid);
            parameters.Add("p_tenant_id", client.TenantId, DbType.Guid);
            parameters.Add("p_first_name", client.FirstName, DbType.String);
            parameters.Add("p_family_name", client.FamilyName, DbType.String);
            parameters.Add("p_status", client.Status, DbType.Int16);
            parameters.Add("p_city", client.Address.City, DbType.String);
            parameters.Add("p_street", client.Address.Street, DbType.String);
            parameters.Add("p_building_number", client.Address.BuildingNumber, DbType.String);
            parameters.Add("p_address_lines", JsonConvert.SerializeObject(client.Address.AddressLines));
            parameters.Add("p_primary_phone_number", client.ContactDetails.PrimaryPhoneNumber, DbType.String);
            parameters.Add("p_secondary_phone_number", client.ContactDetails.SecondaryPhoneNumber, DbType.String);
            parameters.Add("p_email_address", client.ContactDetails.EmailAddress, DbType.String);

            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync("update_client", parameters,
                        transaction: transaction, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Could not update client entity due to error : {exception.Message}");
                    transaction.Rollback();
                    throw;
                }
            }

            return client;
        }

        public async Task DeleteClientAsync(Guid TenantId, Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId == default || clientId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (TenantId == default || TenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(TenantId));
            }

            // use dapper to delete client
            var parameters = new DynamicParameters();
            parameters.Add("id", clientId, DbType.Guid);
            parameters.Add("tenant_id", TenantId, DbType.Guid);

            var sqlCommand = "DELETE FROM clients WHERE id=@id AND tenant_id=@tenant_id";

            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync(sqlCommand, parameters, transaction: transaction, commandType: CommandType.Text);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Could not delete client entity due to error : {exception.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Client?> GetClientAsyncWithDapper(Guid TenantId, Guid id, CancellationToken cancellationToken)
        {
            var sqlCommand = "SELECT * FROM clients WHERE id=@id AND tenant_id=@TenantId";

            var dynamic = new DynamicParameters();
            dynamic.Add(nameof(id), id);
            dynamic.Add(nameof(TenantId), TenantId);

            _logger.LogDebug("{sqlCommand} : {sqlParameters}", sqlCommand, dynamic);
            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);

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

        public async Task<IEnumerable<Client>> SearchClientsAsync(Guid tenantId, string firstName, string familyName, string city, CancellationToken cancellationToken)
        {
            var query = new Query("clients")
                .Select();

            // TentantId is a must operator
            query.Where("tenant_id", tenantId);

            if (!string.IsNullOrEmpty(firstName))
            {
                query.WhereContains("first_name", firstName);
            }
            if (!string.IsNullOrEmpty(familyName))
            {
                query.WhereContains("family_name", familyName);
            }
            if (!string.IsNullOrEmpty(city))
            {
                query.WhereContains("city", city);
            }

            using (var connection = new NpgsqlConnection(dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);

                return await policy.ExecuteAsync(async () =>
                {
                    var compiler = new PostgresCompiler();
                    var generatedQuery = compiler.Compile(query);
                    var result = await connection.QueryAsync<Client, Address, ContactDetails, Client>(
                        generatedQuery.Sql,
                        (client, address, contactDetails) =>
                        {
                            client.Address = address;
                            client.ContactDetails = contactDetails;
                            return client;
                        },
                        param: generatedQuery.NamedBindings,
                        splitOn: "city, primary_phone_number");
                    return result;
                });
            };
        }
    }
}
