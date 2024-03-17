using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteTask
{
    public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<Task>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessLogicValidator _businessLogicValidator;
        private readonly ILogger<DeleteTaskCommandHandler> _logger;

        public DeleteTaskCommandHandler(ILogger<DeleteTaskCommandHandler> logger, IBusinessLogicValidator businessLogicValidator, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _businessLogicValidator = businessLogicValidator ?? throw new ArgumentNullException(nameof(businessLogicValidator));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Task>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeleteTaskCommand} for {Name}", nameof(DeleteTaskCommandHandler), request.TaskId);
            }

            // Step 1: Validate the project by Id, can we delete task under this project
            var deleteTaskValidationResult = await _businessLogicValidator.ValidateAsync(new DeleteTasksValidationRequest(request.TaskId, request.ProjectId, request.TenantId), cancellationToken);
            if (deleteTaskValidationResult != null)
            {
                _logger.LogInformation("Tasks cannot be deleted for project {project} under account {account} due to business logic rules violation: {response}",
                                       request.ProjectId, request.TenantId, deleteTaskValidationResult);
                return new Result<Task>(deleteTaskValidationResult);
            }

            await _unitOfWork.TaskItemsRepository.DeleteAsync(request.TaskId, request.ProjectId, request.TenantId, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Deleted task {task} for project {project} under account {TenantId})", request.TaskId, request.ProjectId, request.TenantId);
            }

            return Task.CompletedTask;
        }
    }
}
