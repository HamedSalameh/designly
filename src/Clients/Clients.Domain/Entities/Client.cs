
using Clients.Domain.ValueObjects;

namespace Clients.Domain.Entities
{
    public class Client : Entity
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

        public Address Address { get; set; }

        public Client(string firstName, string familyName, Address address)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"{nameof(firstName)} : must not be null or empty");
            }

            FirstName = firstName;
            FamilyName = familyName;
            Address = address ?? throw new ArgumentNullException(nameof(Address));
        }
    }
}
