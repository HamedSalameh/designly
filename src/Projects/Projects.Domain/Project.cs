using Designly.Shared;
using Designly.Shared.ValueObjects;

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
        /// Registered user, The lead designer or architect of the project
        /// </summary>
        public required Guid ProjectLeadId { get; set; }
        /// <summary>
        /// The client for whom the project is being done
        /// </summary>
        public required Guid ClientId { get; set; }
        
        /// <summary>
        /// Planned or actual start date of the project
        /// </summary>
        public DateOnly? StartDate { get; set; }
        
        public DateOnly? Deadline { get; set; }
        
        public DateOnly? CompletedAt { get; set; }
        
        // Populated by Dapper
        public List<Property> PropertyList;

        //public List<TaskGroup> TaskGroups { get; set; }
        // Populate by Dapper
        public List<TaskItem> TaskItems { get; set; }

        public Project(Guid TenantId, Guid ProjectLeadId, Guid ClientId, string Name) : base(TenantId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException($"{nameof(Name)} : must not be null or empty");
            }
            this.Name = Name;
            this.ProjectLeadId = ProjectLeadId;
            this.ClientId = ClientId;
            
            this.Description = string.Empty;
            this.PropertyList = new List<Property>();
            this.TaskItems = new List<TaskItem>();
            this.StartDate = null;
            this.Deadline = null;
            this.CompletedAt = null;
        }

        // Used by Dapper for automatic object initialization
        public Project()
        {
            Name = Consts.Strings.ValueNotSet;
            Description = Consts.Strings.ValueNotSet;
            StartDate = null;
            Deadline = null;
            CompletedAt = null;
            PropertyList = new List<Property>();
            TaskItems = new List<TaskItem>();
        }

        // Additional
        //public Budget ProjectBudget { get; set; }       // project budget allocated by the customer
        //public Inventory Inventory { get; set; }        // כתב כמויות
        //public Contract Contract { get; set; }
        //public List<Documents> Plans { get; set; }      // תוכניות
        //public List<Documents> Files { get; set; }      // קבצים, תמונות, וידאו
        //public Questionare questionare { get; set; }    // שאלון לקוח
        //public List<Questionare> Answers { get; set; }  // תשובות לשאלון לקוח
        //public List<Documents> Reports { get; set; }    // דוחות
        //public List<Documents> Invoices { get; set; }   // חשבוניות
        //public List<Documents> Receipts { get; set; }   // קבלות
        //public List<Documents> Expenses { get; set; }   // הוצאות
        //public List<Note> Notes { get; set; }      // הערות
    }
}
