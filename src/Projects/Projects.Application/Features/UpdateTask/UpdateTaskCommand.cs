using LanguageExt.Common;
using MediatR;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Application.Features.UpdateTask
{
    public sealed class UpdateTaskCommand : IRequest<Result<Guid>>
    {
        public TenantId TenantId { get; set; }
        public ProjectId ProjectId { get; set; }
        public TaskItemId TaskItemId { get; set; }
        public required string Name { get; set; }
        //public required Guid TaskGroupId { get; set; }
        //public TaskGroup TaskGroup { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public TaskItemStatus taskItemStatus { get; set; }
    }
}
