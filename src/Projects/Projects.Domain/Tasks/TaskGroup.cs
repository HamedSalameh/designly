﻿namespace Projects.Domain.Tasks
{
    public class TaskGroup : Entity
    {
        public required string Name { get; set; }
        public required Guid ProjectId { get; set; }
        public required BasicProject Project { get; set; }
        public List<TaskItem> Tasks { get; set; }

        public TaskGroup(Guid TenantId, Guid ProjectId, string Name) : base(TenantId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
            }
            this.Name = Name;
            this.ProjectId = ProjectId;
            Tasks = new List<TaskItem>();
        }

        public void AddTask(TaskItem task)
        {
            if (task is null)
            {
                throw new ArgumentException($"Invlaid value for {nameof(task)}");
            }

            Tasks.Add(task);
        }
    }

}
