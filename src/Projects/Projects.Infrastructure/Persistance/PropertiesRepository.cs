﻿using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using Polly.Wrap;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Projects.Infrastructure.Persistance
{
    internal sealed class PropertiesRepository : IPropertiesRepository
    {
        private readonly ILogger<PropertiesRepository> _logger;
        private readonly AsyncPolicyWrap policy;
        private readonly IDbConnectionStringProvider _dbConnectionStringProvider;

        public PropertiesRepository(ILogger<PropertiesRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbConnectionStringProvider = dbConnectionStringProvider ?? throw new ArgumentNullException(nameof(dbConnectionStringProvider));

            policy = PollyPolicyFactory.WrappedAsyncPolicies(logger);

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());

            SqlMapper.AddTypeHandler(new DapperProjectIdTypeHandler());
            SqlMapper.AddTypeHandler(new DapperTenantIdTypeHandler());
            SqlMapper.AddTypeHandler(new DapperTaskItemIdTypeHandler());
            SqlMapper.AddTypeHandler(new DapperProjectLeadIdTypeHandler());
            SqlMapper.AddTypeHandler(new DapperClientIdTypeHandler());
            SqlMapper.AddTypeHandler(new PropertyTypeHandler());
        }

        public async Task<Guid> CreatePropertyAsync(Property property, CancellationToken cancellationToken = default)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.TenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }


            // Build the NpgsqlDataSource with the connection string.
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbConnectionStringProvider.ConnectionString);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

            var command = new NpgsqlCommand("create_or_update_property", connection)
            {
                CommandType = CommandType.StoredProcedure,
                Parameters =
                {
                    new("p_id", property.Id),
                    new("p_tenant_id", property.TenantId.Id),
                    new("p_name", property.Name),
                    new("p_property_type", (int) property.PropertyType),
                    new("p_address", NpgsqlDbType.Jsonb) { Value = property.Address.AddressLines },
                    new("p_floors", NpgsqlDbType.Jsonb) { Value = property.Floors },
                    new("p_total_area", NpgsqlDbType.Numeric) { Value = property.TotalArea },
                }
            };

            // Add the output parameter for property Id
            var propertyIdParam = new NpgsqlParameter("p_property_id", NpgsqlTypes.NpgsqlDbType.Uuid)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(propertyIdParam);

            await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                // Execute the stored procedure
                await command.ExecuteNonQueryAsync(cancellationToken);

                // Retrieve the property Id from the output parameter
                var propertyId = (Guid)propertyIdParam.Value;
                property.Id = propertyId;

                await transaction.CommitAsync(cancellationToken);

                return propertyId;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error creating or retrieving property");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> PropertyExistsAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default)
        {
            if (propertyId == Guid.Empty)
            {
                throw new ArgumentException("Invalid value for property Id");
            }

            if (tenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }

            var sqlScript = "select exists(select 1 from properties where id=@p_id and tenant_id=@p_tenant_id)";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_id", propertyId);
            dynamicParameters.Add("p_tenant_id", tenantId.Id);

            return await policy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
                await connection.OpenAsync(cancellationToken);
                return await connection.ExecuteScalarAsync<bool>(sqlScript, dynamicParameters);
            });
        }

        public async Task DeleteAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default)
        {
            if (propertyId == Guid.Empty)
            {
                throw new ArgumentException("Invalid value for property Id");
            }

            if (tenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }

            var sqlScript = "delete from properties where id=@p_id and tenant_id=@p_tenant_id";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_id", propertyId);
            dynamicParameters.Add("p_tenant_id", tenantId.Id);

            await using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();

                try
                {
                    await policy.ExecuteAsync(async () =>
                    {
                        await connection.ExecuteAsync(sqlScript, dynamicParameters, transaction);
                        transaction.Commit();
                    });
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not delete property due to error: {}", exception.Message);
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to check if a property is attached to a project
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="propertyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> IsPropertyAttachedToProject(TenantId tenantId, Guid propertyId, CancellationToken cancellationToken = default)
        {
            if (tenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }

            if (propertyId == Guid.Empty)
            {
                throw new ArgumentException("Invalid value for property Id");
            }

            var sqlScript = "select exists(select 1 from projects where tenant_id=@p_tenant_id and property_id=@p_property_id)";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_tenant_id", tenantId.Id);
            dynamicParameters.Add("p_property_id", propertyId);

            await using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                try
                {
                    var isPropertyAttachedToProject = policy.ExecuteAsync(async () =>
                    {
                        return await connection.ExecuteScalarAsync<bool>(sqlScript, dynamicParameters);
                    });

                    return await isPropertyAttachedToProject;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not check if property is attached to project due to error: {err}", exception.Message);
                    throw;
                }
            }
        }
    }
}