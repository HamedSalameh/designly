using LanguageExt.Common;
using MediatR;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Application.Features.CreateTask
{
    public sealed class CreateTaskCommand : IRequest<Result<Guid>>
    {
        public TenantId TenantId { get; set; }
        public required string Name { get; set; }
        public required ProjectId ProjectId { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public TaskItemStatus taskItemStatus { get; set; }
    }
}
