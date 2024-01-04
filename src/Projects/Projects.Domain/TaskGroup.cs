namespace Projects.Domain
{
    public class TaskGroup : Entity
    {
        public required string Name { get; set; }
        public required Guid ProjectId { get; set; }
        public required Project Project { get; set; }
        public List<TaskItem> Tasks { get; set; }

        public TaskGroup(Guid TenantId, Guid ProjectId, string Name) : base(TenantId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
            }
            this.Name = Name;
            this.ProjectId = ProjectId;
        }

        public void AddTask(TaskItem task)
        {
            if (task is null || task == default)
            {
                throw new ArgumentException($"Invlaid value for {nameof(task)}");
            }

            Tasks.Add(task);
        }
    }

}
