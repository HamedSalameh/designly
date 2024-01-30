using Projects.Domain;

namespace Projects.Infrastructure.Interfaces
{
    public interface IProjectsRepository
    {
        public Task<Guid> CreateBasicProjectAsync(BasicProject basicProject, CancellationToken cancellationToken = default);
    }
}
