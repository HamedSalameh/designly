﻿using Designly.Auth.Identity;
using Projects.Domain;

namespace Projects.Application.Builders
{
    /// <summary>
    /// A project builder that allows for the construction of a project
    /// <br>The builder will automatically inject the tenant Id from the context</br>
    /// </summary>
    public class ProjectBuilder : IProjectBuilder
    {
        private readonly ITenantProvider _tenantProvider;

        private Guid _projectLeadId;
        private Guid _clientId;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private DateOnly? _startDate;
        private DateOnly? _deadline;
        private DateOnly? _completedAt;

        public ProjectBuilder(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        }

        public IProjectBuilder WithProjectLead(Guid projectLeadId)
        {
            if (projectLeadId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(projectLeadId)} : must not be empty");
            }
            this._projectLeadId = projectLeadId;
            return this;
        }

        public IProjectBuilder WithClient(Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(clientId)} : must not be empty");
            }
            this._clientId = clientId;
            return this;
        }

        public IProjectBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} : must not be null or empty");
            }
            this._name = name;
            return this;
        }

        public IProjectBuilder WithDescription(string description)
        {
            this._description = description;
            return this;
        }

        public IProjectBuilder WithStartDate(DateOnly? startDate)
        {
            if (startDate > _deadline)
            {
                throw new ArgumentException($"{nameof(startDate)} : must be before {nameof(_deadline)}");
            }
            this._startDate = startDate;
            return this;
        }

        public IProjectBuilder WithDeadline(DateOnly? deadline)
        {
            if (_startDate > deadline)
            {
                throw new ArgumentException($"{nameof(deadline)} : must be after {nameof(_startDate)}");
            }
            _deadline = deadline;
            return this;
        }

        public IProjectBuilder WithCompletedAt(DateOnly? completedAt)
        {
            if (completedAt < _startDate)
            {
                throw new ArgumentException($"{nameof(completedAt)} : must be after {nameof(_startDate)}");
            }
            _completedAt = completedAt;
            return this;
        }

        public BasicProject BuildBasicProject()
        {
            var tenantId = _tenantProvider.GetTenantId();

            var basicProject = new BasicProject(tenantId, _projectLeadId, _clientId, _name);
            basicProject.SetDescription(_description);
            if (_startDate.HasValue) basicProject.SetStartDate(_startDate.Value);
            if (_deadline.HasValue) basicProject.SetDeadline(_deadline.Value);
            if (_completedAt.HasValue) basicProject.CompleteProject(_completedAt.Value);
            return basicProject;
        }
    }
}
