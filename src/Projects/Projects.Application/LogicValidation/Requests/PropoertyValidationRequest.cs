using Projects.Domain.StonglyTyped;

namespace Projects.Application.LogicValidation.Requests
{
    public record PropertyValidationRequest(Guid PropertyId, TenantId TenantId) : IBusinessLogicValidationRequest;
}
