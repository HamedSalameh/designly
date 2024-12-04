using Designly.Auth.Identity;
using Designly.Shared.ValueObjects;
using LanguageExt.Pipes;
using Projects.Domain;

namespace Projects.Application.Builders
{
    public class PropertyBuilder : IPropertyBuilder
    {
        private readonly ITenantProvider _tenantProvider;

        public PropertyBuilder(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        }

        private Guid? _id;
        private string? _name = string.Empty;
        private PropertyType _propertyType;
        private Address _address;
        private List<Floor>? _floors;
        private double _totalArea;

        public PropertyBuilder()
        {
        }

        public Property BuildProperty()
        {
            if (_address is null)
            {
                throw new ArgumentException($"{nameof(_address)} : has not been set");
            }

            if (_totalArea <= 0)
            {
                throw new ArgumentException($"{nameof(_totalArea)} : must be greater than 0");
            }

            return new Property(_tenantProvider.GetTenantId(), _name, _propertyType, _address, _floors ?? [])
            {
                TotalArea = _totalArea,
                Id = _id ?? Guid.NewGuid()
            };

        }
        public IPropertyBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.Empty;
            return this;
        }

        public IPropertyBuilder WithName(string? name)
        {
            _name = name;
            return this;
        }

        public IPropertyBuilder WithPropertyType(PropertyType propertyType)
        {
            _propertyType = propertyType;
            return this;
        }

        public IPropertyBuilder WithAddress(Address address)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
            return this;
        }

        public IPropertyBuilder WithFloors(List<Floor>? floors)
        {
            _floors = floors;
            return this;
        }

        public IPropertyBuilder WithTotalArea(double totalArea)
        {
            if (totalArea <= 0)
            {
                throw new ArgumentException($"{nameof(totalArea)} : must be greater than 0");
            }
            _totalArea = totalArea;
            return this;
        }
    }
}
