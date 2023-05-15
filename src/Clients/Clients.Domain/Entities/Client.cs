
using Clients.Domain.ValueObjects;
using System.Text;

namespace Clients.Domain.Entities
{
    public class Client : Entity
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

        public Address Address { get; set; }

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

        private Client(string firstName, string familyName, Guid TenantId) : base(TenantId)
        {
            FirstName = firstName;
            FamilyName = familyName;

            ContactDetails = new ContactDetails(Consts.Strings.ValueNotSet);
            Address = new Address(Consts.Strings.ValueNotSet);
        }

        // Used by Dapper for automatic object initialization
        private Client() : this(Consts.Strings.ValueNotSet, Consts.Strings.ValueNotSet, Guid.Empty)
        {
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

            if (client?.ContactDetails != null)
            {
                UpdateContactDetails(client.ContactDetails);
            }

            if (client?.Address != null)
            {
                UpdateAddress(client.Address);
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            FirstName = client.FirstName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            FamilyName = client.FamilyName;

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
