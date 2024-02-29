using Projects.Domain.StonglyTyped;

namespace Projects.Application.LogicValidation.Requests
{
    public record ClientValidationRequest(ClientId ClientId, TenantId TenantId) : IBusinessLogicValidationRequest;
}
