namespace Projects.Domain
{
    public class Floor
    {
        public static readonly ushort DefaultFloor = 0;

        public int FloorNumber { get; set; }
        public double Area { get; set; }
        
        // Out of scope of MVP
        //public List<PropertySpace> Spaces { get; set; }

        public Floor() : this(DefaultFloor)
        {
            
        }

        public Floor(int FloorNumber = 0)
        {
            this.FloorNumber = FloorNumber;
            //Spaces = [];
        }

        public Floor(int FloorNumber, double Area) : this(FloorNumber)
        {
            this.Area = Area;
        }
    }
}
