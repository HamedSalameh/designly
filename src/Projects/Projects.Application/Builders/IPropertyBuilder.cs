using Designly.Shared.ValueObjects;
using Projects.Domain;

namespace Projects.Application.Builders
{
    public interface IPropertyBuilder
    {
        Property BuildProperty();
        IPropertyBuilder WithId(Guid? id);
        IPropertyBuilder WithName(string? name);
        IPropertyBuilder WithPropertyType(PropertyType propertyType);
        IPropertyBuilder WithAddress(Address address);
        IPropertyBuilder WithFloors(List<Floor>? floors);
        IPropertyBuilder WithTotalArea(double totalArea);
    }
}
