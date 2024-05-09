using Projects.Domain;
using Projects.Infrastructure.Filter;
using System.Collections.Immutable;

namespace Projects.Application.Features.SearchProjects
{
    public static class SupportedProjectFieldNames
    {
        // Switched to ImmutableDictionary from ConcurrentDictionary
        // As there is no need to modify the dictionary after initialization, ImmutableDictionary is a better choice
        public static readonly ImmutableDictionary<string, string> ProjectFieldNamesDictionary = 
            ImmutableDictionary<string, string>.Empty
                .Add(nameof(BasicProject.Id), ProjectFieldToColumnMapping.Id)
                .Add(nameof(BasicProject.Name), ProjectFieldToColumnMapping.ProjectName)
                .Add(nameof(BasicProject.Description), ProjectFieldToColumnMapping.ProjectDescription)
                .Add(nameof(BasicProject.ProjectLeadId), ProjectFieldToColumnMapping.ProjectLead)
                .Add(nameof(BasicProject.ClientId), ProjectFieldToColumnMapping.ClientId)
                .Add(nameof(BasicProject.StartDate), ProjectFieldToColumnMapping.StartDate)
                .Add(nameof(BasicProject.Deadline), ProjectFieldToColumnMapping.Deadline)
                .Add(nameof(BasicProject.CompletedAt), ProjectFieldToColumnMapping.CompletedAt)
                .Add(nameof(BasicProject.Status), ProjectFieldToColumnMapping.Status)
                .Add(nameof(BasicProject.CreatedAt), ProjectFieldToColumnMapping.CreatedAt)
                .Add(nameof(BasicProject.ModifiedAt), ProjectFieldToColumnMapping.ModifiedAt);
    }
}