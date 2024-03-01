using Designly.Auth.Providers;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Builders;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Requests;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProjectBuilder _projectBuilder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessLogicValidator _businessLogicValidator;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
            ITokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            IProjectBuilder projectBuilder,
            IUnitOfWork unitOfWork,
            IBusinessLogicValidator businessLogicValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _projectBuilder = projectBuilder ?? throw new ArgumentNullException(nameof(projectBuilder));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _businessLogicValidator = businessLogicValidator ?? throw new ArgumentNullException(nameof(_businessLogicValidator));
        }

        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateProjectCommand} for {Name}", nameof(CreateProjectCommandHandler), request.Name);
            }

            try
            {
                // Step 1: Validate the customer by Id
                var clientValidationResult = await _businessLogicValidator.ValidateAsync(new ClientValidationRequest(request.ClientId, request.TenantId), cancellationToken);
                if (clientValidationResult != null)
                {
                    _logger.LogInformation("Client validation failed for {ClientId} under account {TenantId}", request.ClientId, request.TenantId);
                    return new Result<Guid>(clientValidationResult);
                }

                // Step 2: Validate the project lead by Id
                var projectLeadValidationResult = await _businessLogicValidator.ValidateAsync(new ProjectLeadValidationRequest(request.ProjectLeadId, request.TenantId), cancellationToken);
                if (projectLeadValidationResult != null)
                {
                    _logger.LogInformation("Project lead validation failed for {ProjectLeadId} under account {TenantId}", request.ProjectLeadId, request.TenantId);
                    return new Result<Guid>(projectLeadValidationResult);
                }

                var projectBuilder = _projectBuilder
                    .WithProjectLead(request.ProjectLeadId)
                    .WithClient(request.ClientId)
                    .WithName(request.Name)
                    .WithDescription(request.Description)
                    .WithStartDate(request.StartDate)
                    .WithDeadline(request.Deadline)
                    .WithCompletedAt(request.CompletedAt);

                var basicProject = projectBuilder.BuildBasicProject();

                var project_id = await _unitOfWork.ProjectsRepository.CreateBasicProjectAsync(basicProject, cancellationToken);

                _logger.LogDebug("Created project: {Name} ({Id}, under account {TenantId})",
                    basicProject.Name, basicProject.Id, basicProject.TenantId);

                return project_id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create project due to error : {ex.Message}", ex.Message);
                throw;
            }
        }
    }
}
