using LanguageExt.Common;
using MediatR;

namespace Accounts.Application.Features.ValidateUser
{
    public record ValidateUserCommand(Guid userId, Guid tenantId) : IRequest<Result<bool>>;
}
