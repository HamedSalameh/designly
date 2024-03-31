using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Infrastructure.Interfaces
{
    public interface ITaskItemsRepository
    {
        Task<Guid> AddAsync(TaskItem taskItem, CancellationToken cancellationToken);
        Task DeleteAsync(TaskItemId taskItemId, ProjectId project, TenantId tenantId, CancellationToken cancellationToken);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken);
        Task<TaskItem?> GetByIdAsync(TenantId tenantId, ProjectId projectId, TaskItemId taskItemId, CancellationToken cancellationToken);
        Task<IEnumerable<TaskItem>> Search(TenantId tenantId, string sqlQuery, CancellationToken cancellationToken);
    }
}
