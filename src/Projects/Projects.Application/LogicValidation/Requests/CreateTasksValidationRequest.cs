using Projects.Domain.StonglyTyped;

namespace Projects.Application.LogicValidation.Requests
{
    public record CreateTasksValidationRequest(ProjectId ProjectId, TenantId TenantId) : IBusinessLogicValidationRequest;
}
