using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteProperty
{
    public sealed record DeletePropertyValidationRequest(Guid PropertyId, TenantId TenantId) : IBusinessLogicValidationRequest
    {
    }
}
