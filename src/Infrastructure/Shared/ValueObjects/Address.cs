using System.Text;

namespace Designly.Shared.ValueObjects
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; } = "N/A";
        public string BuildingNumber { get; set; } = "N/A";
        public List<string>? AddressLines { get; set; }

        public Address(string city, string? street = "", string buildingNumber = "", List<string>? addressLines = null)
        {
            City = city;
            Street = street ?? "";
            BuildingNumber = buildingNumber ?? "";
            AddressLines = addressLines ?? []; ;
        }

        private Address()
        {
            City = Consts.Strings.ValueNotSet;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (!string.IsNullOrEmpty(Street))
            {
                sb.Append($"{Street} St.");
            }

            if (!string.IsNullOrEmpty(BuildingNumber))
            {
                sb.Append((sb.Length > 0 ? ", " : "") + BuildingNumber);
            }

            if (AddressLines != null && AddressLines.Count > 0)
            {
                sb.Append((sb.Length > 0 ? ", " : "") + string.Join(", ", AddressLines));
            }

            if (!string.IsNullOrEmpty(City))
            {
                sb.Append((sb.Length > 0 ? ", " : "") + City);
            }

            return sb.ToString();

        }
    }
}
