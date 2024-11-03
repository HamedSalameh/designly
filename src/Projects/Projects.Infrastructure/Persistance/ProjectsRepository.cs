using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using Polly.Wrap;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;
using SqlKata;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    internal sealed class ProjectsRepository : IProjectsRepository
    {
        private readonly ILogger<ProjectsRepository> _logger;
        private readonly AsyncPolicyWrap policy;
        private readonly IDbConnectionStringProvider _dbConnectionStringProvider;

        public ProjectsRepository(ILogger<ProjectsRepository> logger, IDbConnectionStringProvider dbConnectionStringProvider)
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

        public async Task<BasicProject?> GetByIdAsync(ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken = default)
        {
            if (projectId == ProjectId.Empty)
            {
                throw new ArgumentException("Invalid value for project Id");
            }
            if (tenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }

            var sqlScript = "select * from projects where id=@p_id and tenant_id=@p_tenant_id";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_id", projectId.Id);
            dynamicParameters.Add("p_tenant_id", tenantId.Id);

            await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            return await policy.ExecuteAsync(async () =>
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<BasicProject>(sqlScript,
                        dynamicParameters,
                        transaction: transaction,
                        commandType: CommandType.Text);

                    transaction.Commit();
                    return result;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not retrieve project {Id} under account {TenantId}", projectId, tenantId);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        // UpdateProjectAsync
        // This method uses Npgsql to update a project in the database, by applying the parameters and using a stored procedure
        // The method uses Polly to handle exceptions and retries
        public async Task UpdateAsync(BasicProject basicProject, CancellationToken cancellationToken = default)
        {
            if (basicProject is null)
            {
                _logger.LogError("{BasicProject} is null", nameof(basicProject));
                throw new ArgumentNullException(nameof(basicProject));
            }

            await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            await policy.ExecuteAsync(async () =>
            {
                await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
                try
                {
                    await using var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.StoredProcedure; // Change to CommandType.StoredProcedure for stored procedures
                    command.CommandText = "update_project"; // Ensure this matches your stored procedure name

                    // Map parameters to match the stored procedure
                    command.Parameters.AddWithValue("p_id", NpgsqlDbType.Uuid, basicProject.Id);
                    command.Parameters.AddWithValue("p_tenant_id", NpgsqlDbType.Uuid, basicProject.TenantId.Id);
                    command.Parameters.AddWithValue("p_name", NpgsqlDbType.Varchar, basicProject.Name);
                    command.Parameters.AddWithValue("p_description", NpgsqlDbType.Varchar, basicProject.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_project_lead_id", NpgsqlDbType.Uuid, basicProject.ProjectLeadId.Id);
                    command.Parameters.AddWithValue("p_client_id", NpgsqlDbType.Uuid, basicProject.ClientId.Id);
                    command.Parameters.AddWithValue("p_status", NpgsqlDbType.Integer, (int)basicProject.Status);
                    command.Parameters.AddWithValue("p_start_date", NpgsqlDbType.Date, basicProject.StartDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_deadline", NpgsqlDbType.Date, basicProject.Deadline ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_completed_at", NpgsqlDbType.Date, basicProject.CompletedAt ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_property_id", NpgsqlDbType.Uuid, basicProject.PropertyId ?? (object)DBNull.Value); // Use the property ID or DBNull

                    // Add modified_at parameter
                    command.Parameters.AddWithValue("p_modified_at", NpgsqlDbType.TimestampTz, DateTime.UtcNow); // or use a specific DateTime if needed

                    await command.ExecuteNonQueryAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error updating project {Id} under account {TenantId}", basicProject.Id, basicProject.TenantId);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }


        public async Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default)
        {
            if (basicProject is null)
            {
                _logger.LogError("{BasicProject} is null", nameof(basicProject));
                throw new ArgumentNullException(nameof(basicProject));
            }

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_tenant_id", basicProject.TenantId.Id);
            dynamicParameters.Add("p_name", basicProject.Name);
            dynamicParameters.Add("p_description", basicProject.Description);
            dynamicParameters.Add("p_project_lead_id", basicProject.ProjectLeadId.Id);
            dynamicParameters.Add("p_client_id", basicProject.ClientId.Id);
            dynamicParameters.Add("p_start_date", basicProject.StartDate, DbType.Date);
            dynamicParameters.Add("p_deadline", basicProject.Deadline, DbType.Date);
            dynamicParameters.Add("p_completed_at", basicProject.CompletedAt, DbType.Date);
            dynamicParameters.Add("p_status", basicProject.Status);

            dynamicParameters.Add("p_project_id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            return await policy.ExecuteAsync(async () =>
            {
                using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    await connection.ExecuteAsync(sql: "insert_project",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    var projectId = dynamicParameters.Get<Guid>("p_project_id");
                    basicProject.Id = projectId;

                    await transaction.CommitAsync();

                    return projectId;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error creating basic project");
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        /// <summary>
        /// Deletes a single project, including any and all child entities (task groups, tasks, etc...)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="tenantId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task DeleteProjectAsync(ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken = default)
        {
            if (projectId == ProjectId.Empty)
            {
                throw new ArgumentException("Invalid value for project Id");
            }
            if (tenantId == TenantId.Empty)
            {
                throw new ArgumentException("Invalid value for tenant Id");
            }

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_id", projectId.Id);
            dynamicParameters.Add("p_tenant_id", tenantId.Id);

            await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            await policy.ExecuteAsync(async () =>
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = await connection.BeginTransactionAsync();

                try
                {
                    await connection.ExecuteAsync(sql: "DELETE FROM Projects WHERE id=@p_id AND tenant_id=@p_tenant_id",
                        param: dynamicParameters,
                        commandType: CommandType.Text,
                        transaction: transaction);

                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not delete project {Id} under account {TenantId}", projectId, tenantId);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public async Task<IEnumerable<BasicProject>> SearchProjectsAsync(TenantId tenantId, SqlResult sqlResult, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(tenantId);
            ArgumentNullException.ThrowIfNull(sqlResult);

            var sqlQuery = sqlResult.Sql;
            var parameters = sqlResult.NamedBindings;

            await using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            await connection.OpenAsync(cancellationToken); // Open the connection outside the retry block

            return await policy.ExecuteAsync(async (ct) =>
            {
                try
                {
                    var results = await connection.QueryAsync<BasicProject>(
                        sql: sqlQuery,
                        param: parameters,
                        commandType: CommandType.Text
                    );
                    return results ?? Enumerable.Empty<BasicProject>();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not retrieve projects search results due to error: {Message}", exception.Message);
                    throw;
                }
            }, cancellationToken);
        }

    }
}