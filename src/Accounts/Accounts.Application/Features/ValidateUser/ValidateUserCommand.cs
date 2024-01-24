using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Application.Features.ValidateUser
{
    public record ValidateUserCommand(Guid userId, Guid tenantId) : IRequest<Result<bool>>;
}
