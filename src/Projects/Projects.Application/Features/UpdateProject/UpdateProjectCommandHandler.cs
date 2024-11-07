using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Projects.Application.Builders;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Requests;
using Projects.Domain;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<BasicProject>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessLogicValidator _businessLogicValidator;
        private readonly ILogger<UpdateProjectCommandHandler> _logger;
        private readonly IProjectBuilder _projectBuilder;

        public UpdateProjectCommandHandler(ILogger<UpdateProjectCommandHandler> logger,
                       IUnitOfWork unitOfWork,
                                  IBusinessLogicValidator businessLogicValidator,
                                             IProjectBuilder projectBuilder)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(businessLogicValidator);
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(projectBuilder);

            _logger = logger;
            _unitOfWork = unitOfWork;
            _businessLogicValidator = businessLogicValidator;
            _projectBuilder = projectBuilder;
        }

        public async Task<Result<BasicProject>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            // to un-assign a property, we just the value of property id to null

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {UpdateProjectCommand} for {Name}", nameof(UpdateProjectCommandHandler), request.Name);
            }

            // Step 1: Validate the customer by Id
            var clientValidationResult = await _businessLogicValidator.ValidateAsync(new ClientValidationRequest(request.ClientId, request.TenantId), cancellationToken);
            if (clientValidationResult != null)
            {
                _logger.LogInformation("Client validation failed for {ClientId} under account {TenantId}", request.ClientId, request.TenantId);
                return new Result<BasicProject>(clientValidationResult);
            }

            // Step 2: Validate the project lead by Id
            var projectLeadValidationResult = await _businessLogicValidator.ValidateAsync(new ProjectLeadValidationRequest(request.ProjectLeadId, request.TenantId), cancellationToken);
            if (projectLeadValidationResult != null)
            {
                _logger.LogInformation("Project lead validation failed for {ProjectLeadId} under account {TenantId}", request.ProjectLeadId, request.TenantId);
                return new Result<BasicProject>(projectLeadValidationResult);
            }

            // Step 3: Validate the property by Id
            if (request.PropertyId.HasValue)
            {
                var propertyValidationResult = await _businessLogicValidator.ValidateAsync(new PropertyValidationRequest(request.PropertyId.Value, request.TenantId), cancellationToken);
                if (propertyValidationResult != null)
                {
                    _logger.LogInformation("Property validation failed for {PropertyId} under account {TenantId}", request.PropertyId, request.TenantId);
                    return new Result<BasicProject>(propertyValidationResult);
                }
            }

            var projectValidationResult = await _businessLogicValidator.ValidateAsync(new UpdateProjectValidationRequest(request.TenantId, request.ProjectId), cancellationToken);
            if (projectValidationResult != null)
            {
                _logger.LogInformation("Project {Project} cannot be updated under account {Account} due to business logic rules violation: {Response}",
                    request.ProjectId, request.TenantId, projectValidationResult);
                return new Result<BasicProject>(projectValidationResult);
            }

            var basicProject = _projectBuilder
                    .WithProjectLead(request.ProjectLeadId)
                    .WithClient(request.ClientId)
                    .WithName(request.Name)
                    .WithDescription(request.Description)
                    .WithStartDate(request.StartDate)
                    .WithDeadline(request.Deadline)
                    .WithCompletedAt(request.CompletedAt)
                    .WithProperty(request.PropertyId)
                    .BuildBasicProject();

            // Since this is an update flow of an existing project we need to set the id
            basicProject.SetId(request.ProjectId);

            await _unitOfWork.ProjectsRepository.UpdateAsync(basicProject, cancellationToken).ConfigureAwait(false);

            return new Result<BasicProject>(basicProject);
        }
    }
}
