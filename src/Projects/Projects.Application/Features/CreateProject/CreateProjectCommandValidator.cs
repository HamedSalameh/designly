using FluentValidation;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(p => p.TenantId).NotEmpty().WithMessage("Tenant is required.");

            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            
            RuleFor(p => p.ProjectLeadId).NotEmpty().WithMessage("Project Lead is required.");
            
            RuleFor(p => p.ClientId).NotEmpty().WithMessage("Client is required.");
        }
    }
}
