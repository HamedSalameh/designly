using LanguageExt.Common;
using MediatR;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommand : IRequest<Result<Guid>>
    {
        public CreateAccountCommand(string name, string ownerFirstName, string ownerLastName, string ownerEmail, string ownerJobTitle, string ownerPassword)
        {
            Name = name;
            OwnerFirstName = ownerFirstName;
            OwnerLastName = ownerLastName;
            OwnerEmail = ownerEmail;
            OwnerJobTitle = ownerJobTitle;
            OwnerPassword = ownerPassword;
        }

        public string Name { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerJobTitle { get; set; }
        public string OwnerPassword { get; set; }
    }
}