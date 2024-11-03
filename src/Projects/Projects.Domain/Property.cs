using Designly.Shared;
using Designly.Shared.ValueObjects;

namespace Projects.Domain
{
    public class Property : Entity
    {
        public string? Name { get; set; }
        public PropertyType PropertyType { get; set; } = PropertyType.Unset;
        public Address Address { get; set; }
        public List<Floor>? Floors { get; set; } = new List<Floor>();
        public int NumberOfFloors => Floors?.Count ?? 0;
        public double TotalArea { get; set; }

        public Property(Guid TenantId, string Name, Address Address, List<Floor> Floors)
        {
            this.Name = Name;
            this.Address = Address ?? throw new ArgumentNullException(nameof(Address));
            this.Floors = Floors;
            TotalArea = 0;
        }

        // Used by Dapper for automatic object initialization
        protected Property()
        {
            Name = Consts.Strings.ValueNotSet;
            Address = new Address(Consts.Strings.ValueNotSet);
            Floors = [];
            TotalArea = 0;
        }
    }
}
