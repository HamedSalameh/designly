using Projects.Infrastructure.Interfaces;

namespace Projects.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IProjectsRepository ProjectsRepository { get; }
        public ITaskItemsRepository TaskItemsRepository { get; }
        public IPropertiesRepository PropertiesRepository { get; }

        public UnitOfWork(IProjectsRepository projectsRepository, ITaskItemsRepository taskItemsRepository, IPropertiesRepository propertiesRepository)
        {
            ProjectsRepository = projectsRepository ?? throw new ArgumentNullException(nameof(projectsRepository));
            TaskItemsRepository = taskItemsRepository ?? throw new ArgumentNullException(nameof(taskItemsRepository));
            PropertiesRepository = propertiesRepository ?? throw new ArgumentNullException(nameof(propertiesRepository));
        }
    }
}
