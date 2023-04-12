namespace Clients.Domain.ValueObjects
{
    public class Address
    {
        public string City { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public List<string>? AddressLines { get; set; }

        public Address(string city, string? street = "", string? buildingNumber = "", List<string>? addressLines = null)
        {
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
            AddressLines = addressLines ?? new List<string>(); ;
        }
    }
}
