﻿using Dapper;
using Designly.Shared.ConnectionProviders;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using Projects.Domain;
using Projects.Infrastructure.Interfaces;
using Projects.Infrastructure.Polly;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    internal class ProjectsRepository : IProjectsRepository
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
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            
        }

        public Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default)
        {
            if (basicProject is null)
            {
                _logger.LogError($"{nameof(basicProject)} is null");
                throw new ArgumentNullException(nameof(basicProject));
            }

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_tenant_id", basicProject.TenantId);
            dynamicParameters.Add("p_name", basicProject.Name);
            dynamicParameters.Add("p_project_lead_id", basicProject.ProjectLeadId);
            dynamicParameters.Add("p_client_id", basicProject.ClientId);
            dynamicParameters.Add("p_start_date", basicProject.StartDate);
            dynamicParameters.Add("p_deadline", basicProject.Deadline);
            dynamicParameters.Add("p_completed_at", basicProject.CompletedAt);
            dynamicParameters.Add("p_status", basicProject.Status);

            dynamicParameters.Add("p_project_id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            using var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString);
            return policy.ExecuteAsync(async () =>
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync(sql: "projects.insert_project",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    var projectId = dynamicParameters.Get<Guid>("p_project_id");
                    basicProject.Id = projectId;

                    transaction.Commit();

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
    }
}