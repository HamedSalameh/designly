namespace Projects.Application.Features.SearchTasks
{
    // a defined and support field names that the client must use to filter the tasks    
    public static class SupportedFilterFields
    {
        public static readonly string[] SupportedFields =
        [
            FieldNames.ProjectId,
            FieldNames.TaskId,
            FieldNames.TaskName,
            FieldNames.TaskDescription,
            FieldNames.AssignedTo,
            FieldNames.AssignedBy,
            FieldNames.DueDate,
            FieldNames.CompletedAt,
            FieldNames.TaskItemStatus
        ];
    }

    public static class FieldNames
    {
        public const string ProjectId = "ProjectId";
        public const string TaskId = "TaskId";
        public const string TaskName = "TaskName";
        public const string TaskDescription = "TaskDescription";
        public const string AssignedTo = "AssignedTo";
        public const string AssignedBy = "AssignedBy";
        public const string DueDate = "DueDate";
        public const string CompletedAt = "CompletedAt";
        public const string TaskItemStatus = "TaskItemStatus";
    }
}
