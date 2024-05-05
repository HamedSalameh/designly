using Projects.Domain.Tasks;
using Projects.Infrastructure.Filter;
using System.Collections.Concurrent;

namespace Projects.Application.Features.SearchTasks
{
    public static class SupportedTaskItemFieldNames
    {
        public static readonly ConcurrentDictionary<string, string> TaskItemFieldNamesDic = new ConcurrentDictionary<string, string>
        {
            [nameof(TaskItem.ProjectId)] = TaskItemFieldToColumnMapping.ProjectId,
            [nameof(TaskItem.Id)] = TaskItemFieldToColumnMapping.TaskId,
            [nameof(TaskItem.Name)] = TaskItemFieldToColumnMapping.TaskName,
            [nameof(TaskItem.Description)] = TaskItemFieldToColumnMapping.TaskDescription,
            [nameof(TaskItem.AssignedTo)] = TaskItemFieldToColumnMapping.AssignedTo,
            [nameof(TaskItem.AssignedBy)] = TaskItemFieldToColumnMapping.AssignedBy,
            [nameof(TaskItem.DueDate)] = TaskItemFieldToColumnMapping.DueDate,
            [nameof(TaskItem.CompletedAt)] = TaskItemFieldToColumnMapping.CompletedAt,
            [nameof(TaskItem.TaskItemStatus)] = TaskItemFieldToColumnMapping.TaskItemStatus,
            [nameof(TaskItem.CreatedAt)] = TaskItemFieldToColumnMapping.CreatedAt,
            [nameof(TaskItem.ModifiedAt)] = TaskItemFieldToColumnMapping.ModifiedAt
        };
    }
}
