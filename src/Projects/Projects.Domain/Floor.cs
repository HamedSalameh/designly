namespace Projects.Domain
{
    public class Floor
    {
        public int FloorNumber { get; set; }
        public List<PropertySpace> Spaces { get; set; } = new List<PropertySpace>();

        public Floor(int FloorNumber = 1)
        {
            this.FloorNumber = FloorNumber;
            this.Spaces = new List<PropertySpace>();
        }
    }
}
