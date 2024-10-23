using Projects.Domain;

namespace Projects.Application.Features.UpdateProject
{
    public class UpdateProjectRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid ProjectLeadId { get; set; }
        public Guid ClientId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public ProjectStatus Status { get; set; }
        public PropertyDto? Property { get; set; }
    }

    public class PropertyDto
    {
        public PropertyDto()
        {
            TotalArea = 0;
            Floors = [];
            PropertyType = PropertyType.Other;
        }

        public string? Name { get; set; }
        public PropertyType PropertyType { get; set; }
        public AddressDto? Address { get; set; }
        public List<Floor>? Floors { get; set; }
        public double TotalArea { get; set; }
    }

    public record AddressDto(string City, string Street = "", string BuildingNumber = "", List<string>? AddressLines = null);
}
