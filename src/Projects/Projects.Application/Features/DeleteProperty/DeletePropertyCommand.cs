using LanguageExt.Common;
using MediatR;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.DeleteProperty
{
    public record DeletePropertyCommand(TenantId TenantId, Guid PropertyId) : IRequest<Result<Task>>;
}
