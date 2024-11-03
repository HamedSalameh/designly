using LanguageExt.Common;
using MediatR;
using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.UpdateProject
{
    public sealed class UpdateProjectCommand : IRequest<Result<BasicProject>>
    {
        public ProjectId ProjectId { get; set; }
        public TenantId TenantId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public Guid ProjectLeadId { get; set; }
        public Guid ClientId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public ProjectStatus Status { get; set; }
        public Guid? PropertyId { get; set; }
    }
}