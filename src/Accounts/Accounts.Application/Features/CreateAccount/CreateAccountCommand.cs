using MediatR;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid AccountOwner { get; set; }
    }
}