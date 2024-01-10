using FluentValidation;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(p => p.TenantId).NotEmpty().WithMessage("Tenant is required.");

            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.Description).MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.ProjectLeadId).NotEmpty().WithMessage("Project Lead is required.");

            RuleFor(p => p.ClientId).NotEmpty().WithMessage("Client is required.");

            RuleFor(Command => Command).Custom(ValidateDates);
        }

        private void ValidateDates(CreateProjectCommand command, ValidationContext<CreateProjectCommand> context)
        {
            var startDate = command.StartDate;
            var deadline = command.Deadline;
            var completedAt = command.CompletedAt;

            if (startDate.HasValue && deadline.HasValue && startDate.Value > deadline.Value)
            {
                context.AddFailure("Start Date must be before Deadline.");
            }

            if (startDate.HasValue && completedAt.HasValue && startDate.Value > completedAt.Value)
            {
                context.AddFailure("Start Date must be before Completed At.");
            }
        }
    }
}
