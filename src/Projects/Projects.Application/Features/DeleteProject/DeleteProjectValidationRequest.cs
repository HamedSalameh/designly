using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteProject
{
    public sealed record DeleteProjectValidationRequest(ProjectId Project, TenantId TenantId) : IBusinessLogicValidationRequest
    {
    }
}
