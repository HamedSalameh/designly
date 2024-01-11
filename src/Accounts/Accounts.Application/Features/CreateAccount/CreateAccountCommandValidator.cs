using Accounts.Application.Features.CreateAccount;
using FluentValidation;

namespace Projects.Application.Features.CreateProject
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(p => p.TenantId).NotEmpty().WithMessage("Tenant is required.");

            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.AccountOwner).NotEmpty().WithMessage("Account Owner is required.");

        }

    }
}
