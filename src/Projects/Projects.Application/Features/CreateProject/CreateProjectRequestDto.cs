namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectRequestDto
    {
        public Guid ProjectLeadId { get; set; }
        public Guid ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly? StartDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedAt { get; set; }
    }
}
