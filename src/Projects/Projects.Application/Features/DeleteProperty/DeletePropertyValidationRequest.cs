using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteProperty
{
    public sealed record DeletePropertyValidationRequest(TenantId TenantId, Guid PropertyId) : IBusinessLogicValidationRequest
    {
    }
}
