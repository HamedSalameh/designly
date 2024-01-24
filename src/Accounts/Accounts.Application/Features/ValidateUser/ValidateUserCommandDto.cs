namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandDto
    {
        public string Email { get; set; } = null!;
        public Guid tenantId { get; set; }
    }
}