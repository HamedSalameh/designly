using Projects.Domain.Tasks;
using Projects.Infrastructure.Filter;
using System.Collections.Immutable;

namespace Projects.Application.Features.SearchTasks
{
    public static class SupportedTaskItemFieldNames
    {
        // Switched to ImmutableDictionary from ConcurrentDictionary
        // As there is no need to modify the dictionary after initialization, ImmutableDictionary is a better choice
        public static readonly ImmutableDictionary<string, string> TaskItemFieldNamesDictionary = 
            ImmutableDictionary<string, string>.Empty
                .Add(nameof(TaskItem.ProjectId), TaskItemFieldToColumnMapping.ProjectId)
                .Add(nameof(TaskItem.Id), TaskItemFieldToColumnMapping.TaskId)
                .Add(nameof(TaskItem.Name), TaskItemFieldToColumnMapping.TaskName)
                .Add(nameof(TaskItem.Description), TaskItemFieldToColumnMapping.TaskDescription)
                .Add(nameof(TaskItem.AssignedTo), TaskItemFieldToColumnMapping.AssignedTo)
                .Add(nameof(TaskItem.AssignedBy), TaskItemFieldToColumnMapping.AssignedBy)
                .Add(nameof(TaskItem.DueDate), TaskItemFieldToColumnMapping.DueDate)
                .Add(nameof(TaskItem.CompletedAt), TaskItemFieldToColumnMapping.CompletedAt)
                .Add(nameof(TaskItem.TaskItemStatus), TaskItemFieldToColumnMapping.TaskItemStatus)
                .Add(nameof(TaskItem.CreatedAt), TaskItemFieldToColumnMapping.CreatedAt)
                .Add(nameof(TaskItem.ModifiedAt), TaskItemFieldToColumnMapping.ModifiedAt);
    }
}
