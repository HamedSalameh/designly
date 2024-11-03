using Projects.Domain;

namespace Projects.Application.Features.UpdateProject
{
    public class UpdateProjectRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid ProjectLeadId { get; set; }
        public Guid ClientId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public ProjectStatus Status { get; set; }
        public Guid PropertyId { get; set; }
    }
}
