﻿using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Builders;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Requests;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.CreateTask
{
    public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessLogicValidator _businessLogicValidator;
        private readonly ILogger<CreateTaskCommandHandler> _logger;
        private readonly ITaskItemBuilder _taskItemBuilder;

        public CreateTaskCommandHandler(ILogger<CreateTaskCommandHandler> logger, IUnitOfWork unitOfWork, IBusinessLogicValidator businessLogicValidator, ITaskItemBuilder taskItemBuilder)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentNullException.ThrowIfNull(businessLogicValidator, nameof(businessLogicValidator));
            ArgumentNullException.ThrowIfNull(taskItemBuilder, nameof(taskItemBuilder));

            _unitOfWork = unitOfWork;
            _businessLogicValidator = businessLogicValidator;
            _logger = logger;
            _taskItemBuilder = taskItemBuilder;
        }


        public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateTaskCommand} for {Name}", nameof(CreateTaskCommandHandler), request.Name);
            }

            // Step 1: Validate the project by Id, can we create task under this project
            var projectValidationResult = await _businessLogicValidator.ValidateAsync(new CreateTasksValidationRequest(request.ProjectId, request.TenantId), cancellationToken);
            if (projectValidationResult != null)
            {
                _logger.LogInformation("Tasks cannot be created for project {project} under account {account} due to business logic rules violation: {response}",
                    request.ProjectId, request.TenantId, projectValidationResult);
                return new Result<Guid>(projectValidationResult);
            }

            var taskItem = _taskItemBuilder
                .CreateTaskItem(request.Name, request.ProjectId, request.Description)
                .WithAssignedTo(request.AssignedTo)
                .WithAssignedBy(request.AssignedBy)
                .WithDueDate(request.DueDate)
                .WithCompletedAt(request.CompletedAt)
                .WithStatus(request.taskItemStatus)
                .Build();

            var taskId = await _unitOfWork.TaskItemsRepository.AddAsync(taskItem, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Created task {task} for project {project} under account {TenantId})",
                    taskItem.Id, taskItem.ProjectId, taskItem.TenantId);
            }

            return taskId;
        }
    }
}
