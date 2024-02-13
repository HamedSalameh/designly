﻿using Projects.Infrastructure.Interfaces;

namespace Projects.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IProjectsRepository ProjectsRepository { get; }

        public UnitOfWork(IProjectsRepository projectsRepository)
        {
            ProjectsRepository = projectsRepository ?? throw new ArgumentNullException(nameof(projectsRepository));
        }
    }
}
