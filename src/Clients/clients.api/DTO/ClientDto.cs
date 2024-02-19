namespace Clients.API.DTO
{
    public class ClientDto(Guid id, string firstName, string familyName, AddressDto address, ContactDetailsDto contactDetails, Guid tenantId)
    {
        public Guid Id { get; set; } = id;
        public Guid TenantId { get; set; } = tenantId;
        public string FirstName { get; set; } = firstName;
        public string FamilyName { get; set; } = familyName;
        public AddressDto Address { get; set; } = address;
        public ContactDetailsDto ContactDetails { get; set; } = contactDetails;
    }

    public record AddressDto(string City, string Street = "", string BuildingNumber = "", List<string>? AddressLines = null );
    public record ContactDetailsDto(string PrimaryPhoneNumber, string SecondaryPhoneNumber = "", string EmailAddress = "");
}
