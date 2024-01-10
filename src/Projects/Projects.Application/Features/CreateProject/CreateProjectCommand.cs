using MediatR;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommand : IRequest<Guid>
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectLeadId { get; set; }
        public Guid ClientId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedAt { get; set; }
    }
}
