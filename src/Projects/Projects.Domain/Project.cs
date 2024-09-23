using Designly.Shared;
using Designly.Shared.ValueObjects;
using Newtonsoft.Json.Linq;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Domain
{
    public class BasicProject : Entity
    {
        /// <summary>
        /// General name of the project
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// General optional description of the project
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Registered user, The lead designer or architect of the project
        /// </summary>
        public ProjectLeadId ProjectLeadId { get; private set; }
        /// <summary>
        /// The client for whom the project is being done
        /// </summary>
        public ClientId ClientId { get; private set; }

        /// <summary>
        /// Planned or actual start date of the project
        /// </summary>
        public DateOnly? StartDate { get; private set; }

        /// <summary>
        /// Planned or actual deadline of the project
        /// </summary>
        public DateOnly? Deadline { get; private set; }

        /// <summary>
        /// Actual completion date of the project
        /// </summary>
        public DateOnly? CompletedAt { get; private set; }

        public ProjectStatus Status { get; private set; }

        public bool IsCompleted => CompletedAt.HasValue && Status == ProjectStatus.Completed;

        // Address of the project (the property under work)
        public Address Address { get; private set; }


        /// <summary>
        /// List of task items for the project, general tasks
        /// </summary>
        public List<TaskItem> TaskItems { get; set; }

        public BasicProject(TenantId tenantId, ProjectLeadId projectLeadId, ClientId clientId, string projectName) : base(tenantId)
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
            Status = ProjectStatus.NotStarted;
            Address = new Address(Consts.Strings.ValueNotSet);
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
            Address = new Address(Consts.Strings.ValueNotSet);
        }

        public void SetId(ProjectId projectId)
        {
            if (projectId == ProjectId.Empty)
            {
                throw new ArgumentException($"{nameof(projectId)} : must not be empty");
            }
            Id = projectId;
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

        /// <summary>
        /// Marks the project as completed
        /// </summary>
        /// <param name="completionDate">DateOnly value of the actual completion date</param>
        /// <exception cref="ArgumentException"></exception>
        public void CompleteProject(DateOnly completionDate)
        {
            if (completionDate < StartDate)
            {
                throw new ArgumentOutOfRangeException(nameof(completionDate), $"{nameof(completionDate)} : must be after {nameof(StartDate)}");
            }
            CompletedAt = completionDate;
            Status = ProjectStatus.Completed;
        }

        public void SetStartDate(DateOnly startDate)
        {
            if (Deadline.HasValue && startDate > Deadline)
            {
                throw new ArgumentOutOfRangeException(nameof(startDate), $"{nameof(startDate)} : must be before {nameof(Deadline)}");
            }
            if (CompletedAt.HasValue && startDate > CompletedAt)
            {
                throw new ArgumentOutOfRangeException(nameof(startDate), $"{nameof(startDate)} : must be before {nameof(CompletedAt)}");
            }

            StartDate = startDate;
        }

        public void SetDeadline(DateOnly deadline)
        {
            if (StartDate.HasValue && deadline < StartDate)
            {
                throw new ArgumentOutOfRangeException(nameof(deadline), $"{nameof(deadline)} : must be after {nameof(StartDate)}");
            }
            Deadline = deadline;
        }

        public void SetAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            Address = address;
    }

    // Out of scope for MVP
    //public class Project : BasicProject
    //{
    //    // Populated by Dapper
    //    public List<Property> PropertyList;
    //    public List<TaskGroup> TaskGroups { get; set; }

    //    public Project(Guid TenantId, Guid ProjectLeadId, Guid ClientId, string Name) : base(TenantId, ProjectLeadId, ClientId, Name)
    //    {
    //        PropertyList = [];
    //        TaskGroups = [];
    //    }
    //}

    //public class FullProject : Project
    //{
    //    // Additional
    //    //public Budget ProjectBudget { get; set; }       // project budget allocated by the customer
    //    //public Inventory Inventory { get; set; }        // כתב כמויות
    //    //public Contract Contract { get; set; }
    //    //public List<Documents> Plans { get; set; }      // תוכניות
    //    //public List<Documents> Files { get; set; }      // קבצים, תמונות, וידאו
    //    //public Questionare questionare { get; set; }    // שאלון לקוח
    //    //public List<Questionare> Answers { get; set; }  // תשובות לשאלון לקוח
    //    //public List<Documents> Reports { get; set; }    // דוחות
    //    //public List<Documents> Invoices { get; set; }   // חשבוניות
    //    //public List<Documents> Receipts { get; set; }   // קבלות
    //    //public List<Documents> Expenses { get; set; }   // הוצאות
    //    //public List<Note> Notes { get; set; }      // הערות

    //    public FullProject(Guid TenantId, Guid ProjectLeadId, Guid ClientId, string Name) : base(TenantId, ProjectLeadId, ClientId, Name)
    //    {
    //        PropertyList = [];
    //        TaskGroups = [];
    //    }
    //}
}
