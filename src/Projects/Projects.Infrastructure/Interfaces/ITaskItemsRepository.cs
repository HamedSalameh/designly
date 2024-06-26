﻿using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using SqlKata;

namespace Projects.Infrastructure.Interfaces
{
    public interface ITaskItemsRepository
    {
        Task<Guid> AddAsync(TaskItem taskItem, CancellationToken cancellationToken);
        Task DeleteAsync(TaskItemId taskItemId, ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken);
        Task DeleteAllAsync(ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken);
        Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken);
        Task<TaskItem?> GetByIdAsync(TenantId tenantId, ProjectId projectId, TaskItemId taskItemId, CancellationToken cancellationToken);
        Task<IEnumerable<TaskItem>> SearchAsync(TenantId tenantId, SqlResult sqlResult, CancellationToken cancellationToken);
    }
}
