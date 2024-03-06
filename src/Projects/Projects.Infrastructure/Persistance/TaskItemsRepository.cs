using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    internal sealed class TaskItemsRepository : ITaskItemsRepository
    {
        private readonly ILogger<TaskItemsRepository> _logger;
        private readonly IDbConnectionStringProvider _dbConnectionStringProvider;
        private readonly AsyncPolicyWrap policy;

        public TaskItemsRepository(ILogger<TaskItemsRepository> logger, 
            IDbConnectionStringProvider dbConnectionStringProvider)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(dbConnectionStringProvider);

            _logger = logger;
            policy = PollyPolicyFactory.WrappedAsyncPolicies();
            _dbConnectionStringProvider = dbConnectionStringProvider;

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new JsonbTypeHandler<List<string>>());
        }

        public async Task<Guid> AddAsync(TaskItem taskItem, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(taskItem);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_tenant_id", taskItem.TenantId.Id, DbType.Guid);
            dynamicParameters.Add("p_project_id", taskItem.ProjectId.Id, DbType.Guid);
            dynamicParameters.Add("p_name", taskItem.Name, DbType.String);
            dynamicParameters.Add("p_description", taskItem.Description, DbType.String);
            dynamicParameters.Add("p_assigned_to", taskItem.AssignedTo, DbType.Guid);
            dynamicParameters.Add("p_assigned_by", taskItem.AssignedBy, DbType.Guid);
            dynamicParameters.Add("p_due_date", taskItem.DueDate, DbType.DateTime2);
            dynamicParameters.Add("p_completed_at", taskItem.CompletedAt, DbType.DateTime2);
            dynamicParameters.Add("p_task_item_status", taskItem.taskItemStatus, DbType.Int16);
            dynamicParameters.Add("p_task_item_id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync("create_taskitem", dynamicParameters,
                                               transaction: transaction, 
                                               commandType: CommandType.StoredProcedure);

                    // Retrieve the returned ID from the output parameter
                    var insertedId = dynamicParameters.Get<Guid>("p_task_item_id");
                    taskItem.Id = insertedId;

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not create item due to error : {exception.Message}", exception.Message);
                    transaction.Rollback();
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

                return taskItem.Id;
            }
        }
    }
}
