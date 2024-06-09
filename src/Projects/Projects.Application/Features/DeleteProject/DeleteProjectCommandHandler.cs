using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
    {
        private readonly ILogger<DeleteProjectCommandHandler> _logger;
        private readonly IBusinessLogicValidator _businessLogicValidator;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(ILogger<DeleteProjectCommandHandler> logger, IUnitOfWork unitOfWork, IBusinessLogicValidator businessLogicValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _businessLogicValidator = businessLogicValidator;
        }

        public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeleteProjectCommandHandler} for {ProjectId}", nameof(DeleteProjectCommandHandler), request.ProjectId);
            }

            try
            {
                var deleteProjectValidationRequest = await _businessLogicValidator.ValidateAsync(
                    new DeleteProjectValidationRequest(request.ProjectId, request.TenantId), cancellationToken)
                    .ConfigureAwait(false);
                if (deleteProjectValidationRequest != null)
                {
                    _logger.LogInformation("Project {Project} under account {Account} cannot be deleted due to business logic rules violation: {Response}",
                                                                  request.ProjectId, request.TenantId, deleteProjectValidationRequest);

                    return new Result<bool>(deleteProjectValidationRequest);
                }

                await _unitOfWork.TaskItemsRepository.DeleteAllAsync(request.ProjectId, request.TenantId, cancellationToken);

                await _unitOfWork.ProjectsRepository.DeleteProjectAsync(request.ProjectId, request.TenantId, cancellationToken);

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not delete project due to error: {Message}", exception.Message);
                return new Result<bool>(exception);
            }
        }
    }
}
