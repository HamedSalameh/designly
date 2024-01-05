using Designly.Shared;

namespace Projects.Domain
{
    public class BasicProject : Entity
    {
        /// <summary>
        /// General name of the project
        /// </summary>
        public string Name;

        /// <summary>
        /// General optional description of the project
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Registered user, The lead designer or architect of the project
        /// </summary>
        public Guid ProjectLeadId { get; set; }
        /// <summary>
        /// The client for whom the project is being done
        /// </summary>
        public Guid ClientId;

        /// <summary>
        /// Planned or actual start date of the project
        /// </summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// Planned or actual deadline of the project
        /// </summary>
        public DateOnly? Deadline { get; set; }

        /// <summary>
        /// Actual completion date of the project
        /// </summary>
        public DateOnly? CompletedAt { get; set; }

        public ProjectStatus Status { get => _status; set => _status = value; }

        public bool IsCompleted => CompletedAt.HasValue && Status == ProjectStatus.Completed;

        // Protected and Private fields
        protected ProjectStatus _status;

        /// <summary>
        /// List of task items for the project, general tasks
        /// </summary>
        public List<TaskItem> TaskItems { get; set; }

        public BasicProject(Guid tenantId, Guid projectLeadId, Guid clientId, string projectName) : base(tenantId)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException($"{nameof(projectName)} : must not be null or empty");
            }
            Name = projectName;
            ProjectLeadId = projectLeadId;
            ClientId = clientId;
            Description = string.Empty;
            TaskItems = [];
            StartDate = null;
            Deadline = null;
            CompletedAt = null;
        }

        // Used by Dapper for automatic object initialization
        private BasicProject()
        {
            Name = Consts.Strings.ValueNotSet;
            Description = Consts.Strings.ValueNotSet;
            ProjectLeadId = Guid.Empty;
            ClientId = Guid.Empty;
            StartDate = null;
            Deadline = null;
            CompletedAt = null;
            TaskItems = [];
        }

        public void SetName(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException($"{nameof(projectName)} : must not be null or empty");
            }
            Name = projectName;
        }

        public void SetDescription(string Description)
        {
            this.Description = Description;
        }

        public void SetProjectLeadId(Guid projectLeadId)
        {
            if (projectLeadId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(projectLeadId)} : must not be empty");
            }
            ProjectLeadId = projectLeadId;
        }
    }

    public class Project : BasicProject
    {
        // Populated by Dapper
        public List<Property> PropertyList;
        public List<TaskGroup> TaskGroups { get; set; }

        public Project(Guid TenantId, Guid ProjectLeadId, Guid ClientId, string Name) : base(TenantId, ProjectLeadId, ClientId, Name)
        {
            PropertyList = [];
            TaskGroups = [];
        }
    }

    public class FullProject : Project
    {
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

        public FullProject(Guid TenantId, Guid ProjectLeadId, Guid ClientId, string Name) : base(TenantId, ProjectLeadId, ClientId, Name)
        {
            PropertyList = new List<Property>();
            TaskGroups = new List<TaskGroup>();
        }
    }
}
