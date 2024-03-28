using Dapper;
using Designly.Shared.ConnectionProviders;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly.Wrap;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;
using System.Data;
using static Dapper.SqlMapper;

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
            AddTypeHandler(new JsonbTypeHandler<List<string>>());

            AddTypeHandler(new DapperProjectIdTypeHandler());
            AddTypeHandler(new DapperTenantIdTypeHandler());
            AddTypeHandler(new DapperTaskItemIdTypeHandler());
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
            dynamicParameters.Add("p_due_date", taskItem.DueDate, DbType.DateTime);
            dynamicParameters.Add("p_completed_at", taskItem.CompletedAt, DbType.DateTime);
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

        public async Task DeleteAsync(TaskItemId taskItemId, ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(taskItemId);
            ArgumentNullException.ThrowIfNull(projectId);
            ArgumentNullException.ThrowIfNull(tenantId);

            var sqlScript = "delete from task_items where id = @id and project_id = @project_id and tenant_id = @tenant_id";

            using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync(sqlScript, new { id = taskItemId.Id, project_id = projectId.Id, tenant_id = tenantId.Id },
                                                                      transaction: transaction, 
                                                                      commandType: CommandType.Text);

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
            }
        }

        public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(taskItem);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_tenant_id", taskItem.TenantId.Id, DbType.Guid);
            dynamicParameters.Add("p_project_id", taskItem.ProjectId.Id, DbType.Guid);
            dynamicParameters.Add("p_id", taskItem.Id, DbType.Guid);
            dynamicParameters.Add("p_name", taskItem.Name, DbType.String);
            dynamicParameters.Add("p_description", taskItem.Description, DbType.String);
            dynamicParameters.Add("p_assigned_to", taskItem.AssignedTo, DbType.Guid);
            dynamicParameters.Add("p_assigned_by", taskItem.AssignedBy, DbType.Guid);
            dynamicParameters.Add("p_due_date", taskItem.DueDate, DbType.DateTime);
            dynamicParameters.Add("p_completed_at", taskItem.CompletedAt, DbType.DateTime);
            dynamicParameters.Add("p_task_item_status", taskItem.taskItemStatus, DbType.Int16);

            using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync("update_taskitem", dynamicParameters,
                                               transaction: transaction,
                                               commandType: CommandType.StoredProcedure);

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not update item due to error : {exception.Message}", exception.Message);
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
            }
        }

        public async Task<TaskItem?> GetByIdAsync(TenantId tenantId, ProjectId projectId, TaskItemId taskItemId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(tenantId);
            ArgumentNullException.ThrowIfNull(projectId);
            ArgumentNullException.ThrowIfNull(taskItemId);

            var sqlScript = "select * from task_items where id = @id and project_id = @project_id and tenant_id = @tenant_id";

            using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using var transaction = connection.BeginTransaction();
                try
                {
                    var taskItem = await connection.QueryFirstOrDefaultAsync<TaskItem>(sqlScript, 
                        new { id = taskItemId.Id, project_id = projectId.Id, tenant_id = tenantId.Id },
                        transaction: transaction,
                        commandType: CommandType.Text);

                    transaction.Commit();
                    return taskItem;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Could not retrieve item due to error : {exception.Message}", exception.Message);
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
            }
        }

        public Task<IEnumerable<TaskItem>> Search(string sqlQuery, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery));
            }

            // execute the provided query inside a polling policy
            return policy.ExecuteAsync(async (ct) =>
            {
                using (var connection = new NpgsqlConnection(_dbConnectionStringProvider.ConnectionString))
                {
                    await connection.OpenAsync(ct);
                    using var transaction = connection.BeginTransaction();
                    try
                    {
                        var results = await connection.QueryAsync<TaskItem>(sqlQuery, transaction: transaction, commandType: CommandType.Text);
                        transaction.Commit();
                        return results;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Could not retrieve items due to error : {exception.Message}", exception.Message);
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
                }
            }, cancellationToken);
        }
    }
}
