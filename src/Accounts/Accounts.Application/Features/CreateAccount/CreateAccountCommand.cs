using Accounts.Domain;
using MediatR;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public CreateAccountCommand(string name, string ownerFirstName, string ownerLastName, string ownerEmail, string ownerJobTitle) //, string ownerPhoneNumber)
        {
            Name = name;
            OwnerFirstName = ownerFirstName;
            OwnerLastName = ownerLastName;
            OwnerEmail = ownerEmail;
            OwnerJobTitle = ownerJobTitle;
            //OwnerPhoneNumber = ownerPhoneNumber;
        }

        public string Name { get; set; } = string.Empty;

        public string OwnerFirstName { get; set; } = string.Empty;
        public string OwnerLastName { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public string OwnerJobTitle { get; set; } = string.Empty;
        //public string OwnerPhoneNumber { get; set; } = string.Empty;
    }
}