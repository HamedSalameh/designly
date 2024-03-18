using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.UpdateTask
{
    public sealed record UpdateTaskValidationRequest(TenantId TenantId, ProjectId ProjectId, TaskItemId TaskItemId) : IBusinessLogicValidationRequest
    {
    }
}