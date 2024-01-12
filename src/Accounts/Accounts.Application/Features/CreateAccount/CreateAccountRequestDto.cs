namespace Projects.Application.Features.CreateAccount
{
    public class CreateAccountRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid AccountOwner { get; set; }
    }
}