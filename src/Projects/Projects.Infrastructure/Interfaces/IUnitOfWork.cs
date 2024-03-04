namespace Projects.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IProjectsRepository ProjectsRepository { get; }
        ITaskItemsRepository TaskItemsRepository { get; }
    }
}
