using Designly.Shared;
using Designly.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clients.Domain.Entities
{
    public sealed class Client : Entity
    {
        [Required]
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public ContactDetails ContactDetails { get; set; }

        public ClientStatusCode Status { get; set; }

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
            Status = ClientStatusCode.Active;
        }

        // Used by Dapper for automatic object initialization
        private Client() : base()
        {
            FirstName = Consts.Strings.ValueNotSet;
            FamilyName = Consts.Strings.ValueNotSet;

            ContactDetails = new ContactDetails(Consts.Strings.ValueNotSet);
            Address = new Address(Consts.Strings.ValueNotSet);

            Status = ClientStatusCode.Inactive;
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
            if (client is null)
            {
                throw new ArgumentException($"Invlaid value for {nameof(client)}");
            }
            
            FirstName = client.FirstName;
            FamilyName = client.FamilyName;
            Status = client.Status;

            if (client.ContactDetails != null)
            {
                UpdateContactDetails(client.ContactDetails);
            }

            if (client.Address != null)
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
            sb.Append(", ").Append(Status.ToString());

            return sb.ToString();
        }

        // Class specific implementation of GetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 37  + TenantId.GetHashCode();
                hash = hash * 41 + FirstName.GetHashCode();
                hash = hash * 41 + FamilyName.GetHashCode();
                return hash;
            }
        }

        // Must implement the Equals method to compare entities, hence the IEquatable interface
        public override bool Equals(Entity? other)
        {
            if (other is not Client)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            Client item = (Client)other;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                // Compare the Id and TenantId to ensure uniqueness
                return item.Id == Id && item.TenantId == TenantId;
        }

        // Override the equality operators for convenient usage
        public override bool Equals(object? obj) => Equals(obj as Entity);
    }
}
