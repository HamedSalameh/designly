using Projects.Domain;
using Projects.Domain.StonglyTyped;
using SqlKata;

namespace Projects.Infrastructure.Interfaces
{
    public interface IProjectsRepository
    {
        public Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default);
        public Task DeleteProjectAsync(ProjectId projectId, TenantId tenantId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<BasicProject>> SearchProjectsAsync(TenantId tenantId, SqlResult sqlResult, CancellationToken cancellationToken = default);
    }
}
