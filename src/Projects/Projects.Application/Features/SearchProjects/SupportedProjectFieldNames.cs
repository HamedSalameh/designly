using Projects.Domain;
using Projects.Infrastructure.Filter;
using System.Collections.Concurrent;

namespace Projects.Application.Features.SearchProjects
{
    public static class SupportedProjectFieldNames
    {
        public static readonly ConcurrentDictionary<string, string> ProjectFieldNamesDic = new ConcurrentDictionary<string, string>
        {
            [nameof(BasicProject.Id)] = ProjectFieldToColumnMapping.Id,
            [nameof(BasicProject.Name)] = ProjectFieldToColumnMapping.ProjectName,
            [nameof(BasicProject.Description)] = ProjectFieldToColumnMapping.ProjectDescription,
            [nameof(BasicProject.ProjectLeadId)] = ProjectFieldToColumnMapping.ProjectLead,
            [nameof(BasicProject.ClientId)] = ProjectFieldToColumnMapping.ClientId,
            [nameof(BasicProject.StartDate)] = ProjectFieldToColumnMapping.StartDate,
            [nameof(BasicProject.Deadline)] = ProjectFieldToColumnMapping.Deadline,
            [nameof(BasicProject.CompletedAt)] = ProjectFieldToColumnMapping.CompletedAt,
            [nameof(BasicProject.Status)] = ProjectFieldToColumnMapping.Status,
            [nameof(BasicProject.CreatedAt)] = ProjectFieldToColumnMapping.CreatedAt,
            [nameof(BasicProject.ModifiedAt)] = ProjectFieldToColumnMapping.ModifiedAt
        };
    }
}