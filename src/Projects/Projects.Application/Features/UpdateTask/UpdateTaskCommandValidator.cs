﻿using FluentValidation;
using Projects.Domain;

namespace Projects.Application.Features.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name is required");
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project id is required");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");
            // Since this is an update commend, we expect the task item id to be provided
            RuleFor(x => x.TaskItemId).NotEmpty().WithMessage("Task item id is required");

            RuleFor(x => x.Name).MaximumLength(Constants.TaskItemNameMaxLength).WithMessage("Task name is too long");
            RuleFor(x => x.Description).MaximumLength(Constants.TaskDescriptionMaxLength).WithMessage("Task description is too long");
        }
    }
}
