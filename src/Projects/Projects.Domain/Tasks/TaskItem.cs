using Designly.Shared;

namespace Projects.Domain.Tasks
{
    public class TaskItem : Entity
    {
        public required string Name { get; set; }
        //public required Guid TaskGroupId { get; set; }
        //public TaskGroup TaskGroup { get; set; }
        public required Guid ProjectId { get; set; }
        //public required Project Project { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public TaskItemStatus taskItemStatus { get; set; }
        public bool IsCompleted => taskItemStatus == TaskItemStatus.Completed;

        public TaskItem(Guid TenantId, Guid ProjectId, string Name, string? Description) : base(TenantId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
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
    }

}
