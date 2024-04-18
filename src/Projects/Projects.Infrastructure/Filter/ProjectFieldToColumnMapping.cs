namespace Projects.Infrastructure.Filter
{
    public static class ProjectFieldToColumnMapping
    {
        public const string ProjectsTable = "projects";

        public const string Id = "id";
        public const string TenantId = "tenant_id";
        public const string ProjectName = "name";
        public const string ProjectDescription = "description";
        public const string ProjectLead = "project_lead_id";
        public const string ClientId = "client_id";
        public const string StartDate = "start_date";
        public const string Deadline = "deadline";
        public const string CompletedAt = "completed_at";
        public const string Status = "status";
        public const string CreatedAt = "created_at";
        public const string ModifiedAt = "modified_at";
    }
}
