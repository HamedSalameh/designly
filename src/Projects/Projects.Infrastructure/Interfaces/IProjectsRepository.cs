using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Infrastructure.Interfaces
{
    public interface IProjectsRepository
    {
        public Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default);
        public Task DeleteProjectAsync(ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken = default);
    }
}
