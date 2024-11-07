using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

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

        public Task<bool> PropertyExistsAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default)
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

            return policy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
                await connection.OpenAsync(cancellationToken);
                return await connection.ExecuteScalarAsync<bool>(sqlScript, dynamicParameters);
            });
        }
    }
}