using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteTask
{
    public sealed record DeleteTasksValidationRequest(TaskItemId TaskItemId, ProjectId Project, TenantId TenantId) : IBusinessLogicValidationRequest
    {
    }
}
