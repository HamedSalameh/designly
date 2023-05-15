namespace Clients.API.DTO
{
    public class ClientDto
    {
        public ClientDto(Guid id, string firstName, string familyName, AddressDto address, ContactDetailsDto contactDetails, Guid tenantId)
        {
            Id = id;
            TenantId = tenantId;
            FirstName = firstName;
            FamilyName = familyName;
            Address = address;
            ContactDetails = contactDetails;
        }

        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public AddressDto Address { get; set; }
        public ContactDetailsDto ContactDetails { get; set; }
    }

    public record AddressDto(string City, string Street = "", string BuildingNumber = "", List<string>? AddressLines = null);
    public record ContactDetailsDto(string PrimaryPhoneNumber, string SecondaryPhoneNumber = "", string EmailAddress = "");
}
