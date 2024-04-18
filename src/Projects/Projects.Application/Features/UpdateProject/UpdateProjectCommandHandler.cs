using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Builders;
using Projects.Application.LogicValidation;
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
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(businessLogicValidator, nameof(businessLogicValidator));
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentNullException.ThrowIfNull(projectBuilder, nameof(projectBuilder));

            _logger = logger;
            _unitOfWork = unitOfWork;
            _businessLogicValidator = businessLogicValidator;
            _projectBuilder = projectBuilder;
        }

        public async Task<Result<BasicProject>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {UpdateProjectCommand} for {Name}", nameof(UpdateProjectCommandHandler), request.Name);
            }

            var projectValidationResult = await _businessLogicValidator.ValidateAsync(new UpdateProjectValidationRequest(request.TenantId, request.ProjectId), cancellationToken);
            if (projectValidationResult != null)
            {
                _logger.LogInformation("Project {project} cannot be updated under account {account} due to business logic rules violation: {response}",
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
                    .BuildBasicProject();

            // Since this is an update flow of an existing project we need to set the id
            basicProject.SetId(request.ProjectId);

            await _unitOfWork.ProjectsRepository.UpdateAsync(basicProject, cancellationToken).ConfigureAwait(false);

            return new Result<BasicProject>(basicProject);
        }
    }
}
