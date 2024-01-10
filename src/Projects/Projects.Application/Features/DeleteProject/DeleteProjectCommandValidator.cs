using FluentValidation;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(p => p.TenantId).NotEmpty().WithMessage("TenantId is required.");

            RuleFor(p => p.ProjectId).NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}
