﻿namespace Projects.Domain
{
    public class Floor
    {
        public int FloorNumber { get; set; }
        public List<PropertySpace> Spaces { get; set; } = [];

        public Floor(int FloorNumber = 1)
        {
            this.FloorNumber = FloorNumber;
            Spaces = new List<PropertySpace>();
        }
    }
}
