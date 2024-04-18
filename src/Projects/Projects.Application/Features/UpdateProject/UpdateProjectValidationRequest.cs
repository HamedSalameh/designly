using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.UpdateProject
{
    public sealed record UpdateProjectValidationRequest(TenantId TenantId, ProjectId ProjectId) : IBusinessLogicValidationRequest
    { }
}