using Designly.Shared;
using Projects.Domain.StonglyTyped;

namespace Projects.Domain.Tasks
{
    public sealed class TaskItem : Entity
    {
        public ProjectId ProjectId { get; private set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TaskItemStatus taskItemStatus { get; private set; }
        public bool IsCompleted => taskItemStatus == TaskItemStatus.Completed;

        public TaskItem(TenantId TenantId, ProjectId ProjectId, string Name, string? Description) : base(TenantId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
            }
            if (TenantId == TenantId.Empty)
            {
                throw new ArgumentException($"Invalid value for {nameof(TenantId)}");
            }
            if (ProjectId == ProjectId.Empty)
            {
                throw new ArgumentException($"Invalid value for {nameof(ProjectId)}");
            }

            this.Name = Name;
            this.Description = Description;
            this.ProjectId = ProjectId;
        }

        // Used by Dapper for automatic object initialization
        private TaskItem() : base()
        {
            Name = Consts.Strings.ValueNotSet;
            Description = Consts.Strings.ValueNotSet;
            taskItemStatus = TaskItemStatus.NotStarted;
            ProjectId = Guid.Empty;
        }

        public void Complete()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("TaskItem is already completed");
            }
            taskItemStatus = TaskItemStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Reopen()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("TaskItem is not completed");
            }
            taskItemStatus = TaskItemStatus.InProgress;
            CompletedAt = null;
        }

        /// <summary>
        /// Updates the status of the task item to the specified status
        /// Supported: NotStarted, InProgress, Completed, OnHold, Cancelled
        /// </summary>
        /// <param name="taskItemStatus"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdateTaskStatus(TaskItemStatus taskItemStatus)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("TaskItem is already completed. Reopen the task to update status");
            }

            this.taskItemStatus = taskItemStatus;
        }

        public void SetId(TaskItemId id)
        {
            if (id == TaskItemId.Empty)
            {
                throw new ArgumentException("Invalid value for TaskItem Id");
            }
            // We need to block attempts to set an Id if the entity is already initialized
            if ( CreatedAt == default || ModifiedAt == default )
            {
                throw new InvalidOperationException("Entity Id must not be set before the entity is created");
            }

            Id = id;
        }
    }

}
