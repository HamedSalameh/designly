using Designly.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Domain
{
    public class Project : Entity
    {
        public required string Name { get; set; }
        
        /// <summary>
        /// General optional description of the project
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// The lead designer or architect of the project
        /// </summary>
        public required Guid ProjectLead { get; set; }
        /// <summary>
        /// The client for whom the project is being done
        /// </summary>
        public required Guid ClientId { get; set; }
        
        /// <summary>
        /// Planned or actual start date of the project
        /// </summary>
        public DateOnly StartDate { get; set; }
        
        public DateOnly Deadline { get; set; }
        
        public DateOnly? CompletedAt { get; set; }
        public required Property Property;
    }

    public class Property : Entity
    {
        public string? Name { get; set; }
        public required Address Address { get; set; }
        public required List<Floor> Floors { get; set; }
        public double TotalArea { get; set; }
    }

    public class Floor
    {
        public int FloorNumber { get; set; }
        public List<AreaMeasurement> AreaMeasurements { get; set; } = new List<AreaMeasurement>();
    }

    /// <summary>
    /// AreaMeasurement class hold the actual measurements of the space within the property (room, floor, etc.)
    /// </summary>
    public class AreaMeasurement
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// A value representing the height of the ceiling in meters
        /// </summary>
        public double CeilingHeight { get; set; }
        /// <summary>
        /// A Value represting the area of the property in square meters
        /// </summary>
        public double MeasuredArea { get; set; }
    }   
}
