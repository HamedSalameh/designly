using Projects.Domain;

namespace Projects.Application.Builders
{
    public interface IProjectBuilder
    {
        BasicProject BuildBasicProject();
        ProjectBuilder WithClient(Guid clientId);
        ProjectBuilder WithName(string name);
        ProjectBuilder WithDescription(string description);
        ProjectBuilder WithProjectLead(Guid projectLeadId);
        ProjectBuilder WithStartDate(DateOnly? startDate);
        ProjectBuilder WithDeadline(DateOnly? deadline);
        ProjectBuilder WithCompletedAt(DateOnly? completedAt);
    }
}
