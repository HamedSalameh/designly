namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string OwnerFirstName { get; set; } = string.Empty;
        public string OwnerLastName { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public string OwnerJobTitle { get; set; } = string.Empty;
        public string OwnerPassword { get; set; } = string.Empty;
        // TODO: Add this
        //public string OwnerPhoneNumber { get; set; } = string.Empty;

    }
}