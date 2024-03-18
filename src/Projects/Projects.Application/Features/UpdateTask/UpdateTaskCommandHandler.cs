using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Builders;
using Projects.Application.LogicValidation;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.UpdateTask
{
    public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskItem>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessLogicValidator _businessLogicValidator;
        private readonly ILogger<UpdateTaskCommandHandler> _logger;
        private readonly ITaskItemBuilder _taskItemBuilder;

        public UpdateTaskCommandHandler(ILogger<UpdateTaskCommandHandler> logger, 
            IUnitOfWork unitOfWork, 
            IBusinessLogicValidator businessLogicValidator, 
            ITaskItemBuilder taskItemBuilder)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(businessLogicValidator, nameof(businessLogicValidator));
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentNullException.ThrowIfNull(taskItemBuilder, nameof(taskItemBuilder));

            _logger = logger;
            _unitOfWork = unitOfWork;
            _businessLogicValidator = businessLogicValidator;
            _taskItemBuilder = taskItemBuilder;
        }

        public async Task<Result<TaskItem>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {UpdateTaskCommand} for {Name}", nameof(UpdateTaskCommandHandler), request.Name);
            }

            var projectValidationResult = await _businessLogicValidator.ValidateAsync(new UpdateTaskValidationRequest(request.TenantId, request.ProjectId, request.TaskItemId), cancellationToken);
            if (projectValidationResult != null)
            {
                _logger.LogInformation("Tasks cannot be updated for project {project} under account {account} due to business logic rules violation: {response}",
                                       request.ProjectId, request.TenantId, projectValidationResult);
                return new Result<TaskItem>(projectValidationResult);
            }

            var taskItem = _taskItemBuilder
                .CreateTaskItem(request.Name, request.ProjectId, request.Description)
                .WithAssignedTo(request.AssignedTo)
                .WithAssignedBy(request.AssignedBy)
                .WithDueDate(request.DueDate)
                .WithCompletedAt(request.CompletedAt)
                .WithStatus(request.taskItemStatus)
                .Build();

            // Since this is an update flow of an existing task item we need to set the id
            taskItem.SetId(request.TaskItemId);

            await _unitOfWork.TaskItemsRepository.UpdateAsync(taskItem, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Task {TaskId} for project {projectId} under account {accountId} has been updated", 
                    taskItem.Id, taskItem.ProjectId, taskItem.TenantId);
            }

            return new Result<TaskItem>(taskItem);
        }
    }
}
