using Projects.Domain.StonglyTyped;

namespace Projects.Application.LogicValidation.Requests
{
    public record ProjectLeadValidationRequest(ProjectLeadId ProjectLeadId, TenantId TenantId) : IBusinessLogicValidationRequest;
}
