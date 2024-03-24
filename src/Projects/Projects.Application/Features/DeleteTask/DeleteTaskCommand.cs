using LanguageExt.Common;
using MediatR;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteTask
{
    public record DeleteTaskCommand(TenantId TenantId, ProjectId ProjectId, TaskItemId TaskId) : IRequest<Result<Task>>
    {
        public Guid TaskItemId { get; set; }
    }
}
