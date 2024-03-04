using Projects.Infrastructure.Interfaces;

namespace Projects.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IProjectsRepository ProjectsRepository { get; }
        public ITaskItemsRepository TaskItemsRepository { get; }

        public UnitOfWork(IProjectsRepository projectsRepository, ITaskItemsRepository taskItemsRepository)
        {
            ProjectsRepository = projectsRepository ?? throw new ArgumentNullException(nameof(projectsRepository));
            TaskItemsRepository = taskItemsRepository ?? throw new ArgumentNullException(nameof(taskItemsRepository));
        }
    }
}
