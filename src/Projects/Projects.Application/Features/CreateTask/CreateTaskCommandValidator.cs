using FluentValidation;
using Projects.Domain;

namespace Projects.Application.Features.CreateTask
{
    public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name is required");
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project id is required");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");

            RuleFor(x => x.Name).MaximumLength(Constants.TaskItemNameMaxLength).WithMessage("Task name is too long");
            RuleFor(x => x.Description).MaximumLength(Constants.TaskDescriptionMaxLength).WithMessage("Task description is too long");
        }
    }
}
