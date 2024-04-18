using FluentValidation;
using Projects.Domain;

namespace Projects.Application.Features.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Project name is required");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project id is required");

            RuleFor(x => x.Name).MaximumLength(Constants.ProjectNameMaxLength).WithMessage("Project name is too long");
            RuleFor(x => x.Description).MaximumLength(Constants.ProjectDescriptionMaxLength).WithMessage("Project description is too long");
        }
    }
}
