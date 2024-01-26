namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandDto
    {
        public Guid userId { get; set; }
        public Guid tenantId { get; set; }
    }
}