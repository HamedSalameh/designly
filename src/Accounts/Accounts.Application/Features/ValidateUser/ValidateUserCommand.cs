using LanguageExt.Common;
using MediatR;

namespace Accounts.Application.Features.ValidateUser
{
    public record ValidateUserCommand(Guid UserId, Guid TenantId) : IRequest<Result<bool>>;
}
