using Projects.Domain;

namespace Projects.Application.Builders
{
    public interface IProjectBuilder
    {
        BasicProject BuildBasicProject();
        ProjectBuilder WithClient(Guid clientId);
        ProjectBuilder WithCompletedAt(DateOnly completedAt);
        ProjectBuilder WithDeadline(DateOnly deadline);
        ProjectBuilder WithDescription(string description);
        ProjectBuilder WithName(string name);
        ProjectBuilder WithProjectLead(Guid projectLeadId);
        ProjectBuilder WithStartDate(DateOnly startDate);
    }
}