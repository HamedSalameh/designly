using Designly.Shared;
using Designly.Shared.ValueObjects;

namespace Projects.Domain
{
    public class Property : Entity
    {
        public string? Name { get; set; }
        public PropertyType PropertyType { get; set; }
        public required Address Address { get; set; }
        public required List<Floor> Floors { get; set; }
        public int NumberOfFloors => Floors.Count;
        public double TotalArea { get; set; }

        public Property(Guid TenantId, string Name, Address Address) : base(TenantId)
        {
            this.Name = Name;
            this.Address = Address;
            Floors = [];
            TotalArea = 0;
        }

        // Used by Dapper for automatic object initialization
        protected Property() : base()
        {
            Name = Consts.Strings.ValueNotSet;
            Address = new Address(Consts.Strings.ValueNotSet);
            Floors = [];
            TotalArea = 0;
        }
    }
}
