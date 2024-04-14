namespace Projects.Infrastructure.Filter
{
    public static class TaskItemFieldToColumnMapping
    {
        public const string TaskItemTable = "task_items";

        public const string ProjectId = "project_id";
        public const string TaskId = "task_id";
        public const string TaskName = "name";
        public const string TaskDescription = "task_description";
        public const string AssignedTo = "assigned_to";
        public const string AssignedBy = "assigned_by";
        public const string DueDate = "due_date";
        public const string CompletedAt = "completed_at";
        public const string TaskItemStatus = "task_item_status";
        public const string CreatedAt = "created_at";
        public const string ModifiedAt = "modified_at";
    }
}
