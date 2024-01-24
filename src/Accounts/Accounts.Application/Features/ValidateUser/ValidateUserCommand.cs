using LanguageExt.Common;
using MediatR;

namespace Accounts.Application.Features.ValidateUser
{
    public record ValidateUserCommand(string Email, Guid tenantId) : IRequest<Result<bool>>;
}
