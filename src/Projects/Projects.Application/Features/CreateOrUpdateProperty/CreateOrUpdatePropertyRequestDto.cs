namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyRequestDto
    {
        public Guid? Id { get; set; } = null;
        public string? Name { get; set; }
        public int PropertyType { get; set; }
        public AddressDto Address { get; set; }
        public List<FloorDto>? Floors { get; set; }
        public double TotalArea { get; set; }
    }

    public record AddressDto(string City, string Street = "", string BuildingNumber = "", List<string>? AddressLines = null);

    public class FloorDto
    {
        public int FloorNumber { get; set; }
        public double Area { get; set; }
    }
}
