using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
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

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
            SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
            policy = PollyPolicyFactory.WrappedAsyncPolicies();

        }

        public async Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default)
        {
            if (basicProject is null)
            {
                _logger.LogError($"{nameof(basicProject)} is null");
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

            using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            return await policy.ExecuteAsync(async () =>
            {
                await connection.OpenAsync(cancellationToken);
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
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        await connection.CloseAsync();
                        connection.Dispose();
                    }
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

            using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
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
                    _logger.LogError(exception, "Could not delete project {id} under account {tenant}", projectId, tenantId);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        await connection.CloseAsync();
                        connection.Dispose();
                    }
                }
            });

            return;
        }

        public Task<IEnumerable<BasicProject>> SearchProjectsAsync(TenantId tenantId, SqlResult sqlResult, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }


}
