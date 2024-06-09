using LanguageExt.Common;
using MediatR;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteProject
{
    public record DeleteProjectCommand(ProjectId ProjectId, TenantId TenantId) : IRequest<Result<bool>>
    {
    }
}