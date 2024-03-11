using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Application.Builders
{
    public interface ITaskItemBuilder
    {
        ITaskItemBuilder CreateTaskItem(string Name, ProjectId projectId, string? Description);
        ITaskItemBuilder WithAssignedTo(Guid? Assignee);
        ITaskItemBuilder WithAssignedBy(Guid? Assigner);
        ITaskItemBuilder WithDueDate(DateTime? DueDate);
        ITaskItemBuilder WithCompletedAt(DateTime? CompletedAt);
        ITaskItemBuilder WithStatus(TaskItemStatus taskItemStatus);
        TaskItem Build();
    }
}
