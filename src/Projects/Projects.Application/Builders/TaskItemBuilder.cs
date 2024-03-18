using Designly.Auth.Identity;
using Designly.Base.Exceptions;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Application.Builders
{
    public class TaskItemBuilder : ITaskItemBuilder
    {
        private readonly ITenantProvider _tenantProvider;

        private TaskItem? _taskItem;

        public TaskItemBuilder(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        }

        public TaskItem Build()
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            return _taskItem;
        }

        public ITaskItemBuilder CreateTaskItem(string Name, ProjectId projectId, string? Description)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
            }

            var tenantId = _tenantProvider.GetTenantId();
            _taskItem = new TaskItem(tenantId, projectId, Name, Description);
            return this;
        }

        public ITaskItemBuilder WithAssignedBy(Guid? Assigner)
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            _taskItem.AssignedBy = Assigner;
            return this;
        }

        public ITaskItemBuilder WithAssignedTo(Guid? Assignee)
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            _taskItem.AssignedTo = Assignee;
            return this;
        }

        public ITaskItemBuilder WithCompletedAt(DateTime? CompletedAt)
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            if (!CompletedAt.HasValue)
            {
                throw new BusinessLogicException($"{nameof(CompletedAt)} : must have a valid date value");
            }
            if (CompletedAt.Value.Date > DateTime.UtcNow.Date)
            {
                throw new BusinessLogicException($"{nameof(CompletedAt)} : cannot have a future date value");
            }

            _taskItem.CompletedAt = CompletedAt;
            return this;
        }

        public ITaskItemBuilder WithDueDate(DateTime? DueDate)
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            if (!DueDate.HasValue)
            {
                throw new BusinessLogicException($"{nameof(DueDate)} : must have a valid date value");
            }

            _taskItem.DueDate = DueDate;
            return this;
        }

        public ITaskItemBuilder WithStatus(TaskItemStatus taskItemStatus)
        {
            if (_taskItem == null)
            {
                throw new BusinessLogicException("Task item is not created yet.");
            }

            // If completedAt is not set, set it to current date
            if (taskItemStatus == TaskItemStatus.Completed && !_taskItem.CompletedAt.HasValue)
            {
                _taskItem.CompletedAt = DateTime.UtcNow;
            }

            _taskItem.UpdateTaskStatus(taskItemStatus);
            return this;
        }
    }
}
