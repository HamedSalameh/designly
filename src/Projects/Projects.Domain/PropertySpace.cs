namespace Projects.Domain
{
    /// <summary>
    /// AreaMeasurement class hold the actual measurements of the space within the property (room, floor, etc.)
    /// </summary>
    public class PropertySpace
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// A value representing the height of the ceiling in meters
        /// </summary>
        public double CeilingHeight { get; set; } = 0;
        /// <summary>
        /// A Value represting the area of the property in square meters
        /// </summary>
        public double MeasuredArea { get; set; } = 0;

        public PropertySpace(string Name)
        {
            this.Name = Name;
        }

        public PropertySpace(string Name, string? Description, double CeilingHeight, double MeasuredArea)
        {
            this.Name = Name;
            this.Description = Description;
            this.CeilingHeight = CeilingHeight;
            this.MeasuredArea = MeasuredArea;
        }
    }
}
