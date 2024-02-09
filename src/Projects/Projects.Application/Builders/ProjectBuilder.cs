using Designly.Auth.Identity;
using Projects.Domain;

namespace Projects.Application.Builders
{
    /// <summary>
    /// A project builder that allows for the construction of a project
    /// <br>The builder will automatically inject the tenant Id from the context</br>
    /// </summary>
    public class ProjectBuilder : IProjectBuilder
    {
        private readonly ITenantProvider _tenantProvider;

        private Guid ProjectLeadId;
        private Guid ClientId;
        private string Name = string.Empty;
        private string Description = string.Empty;
        private DateOnly? StartDate;
        private DateOnly? Deadline;
        private DateOnly? CompletedAt;

        public ProjectBuilder(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        }

        public ProjectBuilder WithProjectLead(Guid projectLeadId)
        {
            if (projectLeadId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(projectLeadId)} : must not be empty");
            }
            ProjectLeadId = projectLeadId;
            return this;
        }

        public ProjectBuilder WithClient(Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(clientId)} : must not be empty");
            }
            ClientId = clientId;
            return this;
        }

        public ProjectBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} : must not be null or empty");
            }
            Name = name;
            return this;
        }

        public ProjectBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public ProjectBuilder WithStartDate(DateOnly startDate)
        {
            if (startDate > Deadline)
            {
                throw new ArgumentException($"{nameof(startDate)} : must be before {nameof(Deadline)}");
            }
            StartDate = startDate;
            return this;
        }

        public ProjectBuilder WithDeadline(DateOnly deadline)
        {
            if (StartDate > deadline)
            {
                throw new ArgumentException($"{nameof(deadline)} : must be after {nameof(StartDate)}");
            }
            Deadline = deadline;
            return this;
        }

        public ProjectBuilder WithCompletedAt(DateOnly completedAt)
        {
            if (completedAt < StartDate)
            {
                throw new ArgumentException($"{nameof(completedAt)} : must be after {nameof(StartDate)}");
            }
            CompletedAt = completedAt;
            return this;
        }

        public BasicProject BuildBasicProject()
        {
            var tenantId = _tenantProvider.GetTenantId();

            var basicProject = new BasicProject(tenantId, ProjectLeadId, ClientId, Name);
            basicProject.Description = Description;
            if (StartDate is not null) basicProject.SetStartDate(StartDate.Value);
            if (Deadline is not null) basicProject.SetDeadline(Deadline.Value);
            if (CompletedAt is not null && CompletedAt.HasValue) basicProject.CompleteProject(CompletedAt.Value);
            return basicProject;
        }
    }
}
