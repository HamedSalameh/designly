using FluentValidation;

namespace Projects.Application.Features.DeleteTask
{
    internal class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project id is required");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task id is required");
        }
    }
}
