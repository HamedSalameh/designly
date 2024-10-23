using Projects.Domain;

namespace Projects.Application.Builders
{
    public interface IProjectBuilder
    {
        BasicProject BuildBasicProject();
        IProjectBuilder WithClient(Guid clientId);
        IProjectBuilder WithName(string name);
        IProjectBuilder WithDescription(string description);
        IProjectBuilder WithProjectLead(Guid projectLeadId);
        IProjectBuilder WithStartDate(DateOnly? startDate);
        IProjectBuilder WithDeadline(DateOnly? deadline);
        IProjectBuilder WithCompletedAt(DateOnly? completedAt);
        IProjectBuilder WithProperty(Property? property);
    }
}
