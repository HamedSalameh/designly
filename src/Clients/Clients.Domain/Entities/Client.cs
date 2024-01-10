using Designly.Shared;
using Designly.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clients.Domain.Entities
{
    public class Client : Entity
    {
        [Required]
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public ContactDetails ContactDetails { get; set; }

        public Client(string firstName, string familyName, Address address, ContactDetails contactDetails, Guid TenantId)
            : base(TenantId)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"{nameof(firstName)} : must not be null or empty");
            }
            FirstName = firstName;
            FamilyName = familyName;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            ContactDetails = contactDetails ?? throw new ArgumentNullException(nameof(contactDetails));
        }

        // Used by Dapper for automatic object initialization
        private Client() : base()
        {
            FirstName = Consts.Strings.ValueNotSet;
            FamilyName = Consts.Strings.ValueNotSet;

            ContactDetails = new ContactDetails(Consts.Strings.ValueNotSet);
            Address = new Address(Consts.Strings.ValueNotSet);
        }

        public void UpdateAddress(Address address)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address), "Address cannot be null");
        }

        public void UpdateContactDetails(ContactDetails contactDetails)
        {
            ContactDetails = contactDetails ??
                             throw new ArgumentNullException(nameof(contactDetails), "ContactDetails cannot be null");
        }

        public Client UpdateClient(Client client)
        {
            if (client is null || client == default)
            {
                throw new ArgumentException($"Invlaid value for {nameof(client)}");
            }
            
            FirstName = client.FirstName;
            FamilyName = client.FamilyName;

            if (client?.ContactDetails != null)
            {
                UpdateContactDetails(client.ContactDetails);
            }

            if (client?.Address != null)
            {
                UpdateAddress(client.Address);
            }

            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(FirstName);
            if (!string.IsNullOrEmpty(FamilyName)) sb.Append(' ').Append(FamilyName);

            sb.Append(", ").Append(Address.ToString());
            sb.Append(", ").Append(ContactDetails.ToString());

            return sb.ToString();
        }
    }
}
