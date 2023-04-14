namespace Clients.API.DTO
{
    public record ClientDto(string FirstName, string FamilyName, AddressDto Address, ContactDetailsDto ContactDetails);
    public record AddressDto(string City, string Street = "", string BuildingNumber = "", List<string>? AddressLines = null);
    public record ContactDetailsDto(string PrimaryPhoneNumber, string SecondaryPhoneNumber = "", string EmailAddress = "");
}
