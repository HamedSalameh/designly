using Accounts.Domain;
using FluentValidation;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Name).MaximumLength(Consts.AccountNameMaxLength).WithMessage($"Name must not exceed {Consts.AccountNameMaxLength} characters.");
            RuleFor(p => p.Name).MinimumLength(Consts.AccountNameMinLength).WithMessage($"Name must be at least {Consts.AccountNameMinLength} characters.");

            RuleFor(p => p.OwnerFirstName).NotEmpty().WithMessage("Owner first name is required.");
            RuleFor(p => p.OwnerFirstName).MaximumLength(Designly.Shared.Consts.FirstNameMaxLength).WithMessage($"Owner first name must not exceed {Designly.Shared.Consts.FirstNameMaxLength} characters.");
            RuleFor(p => p.OwnerFirstName).MinimumLength(Designly.Shared.Consts.FirstNameMinLength).WithMessage($"Owner first name must be at least {Designly.Shared.Consts.FirstNameMinLength} characters.");

            RuleFor(p => p.OwnerLastName).NotEmpty().WithMessage("Owner last name is required.");
            RuleFor(p => p.OwnerLastName).MaximumLength(Designly.Shared.Consts.LastNameMaxLength).WithMessage($"Owner last name must not exceed {Designly.Shared.Consts.LastNameMaxLength} characters.");
            RuleFor(p => p.OwnerLastName).MinimumLength(Designly.Shared.Consts.LastNameMinLength).WithMessage($"Owner last name must be at least {Designly.Shared.Consts.LastNameMinLength} characters.");

            RuleFor(p => p.OwnerEmail).NotEmpty().WithMessage("Owner email is required.");
            RuleFor(p => p.OwnerEmail).MaximumLength(Designly.Shared.Consts.MaxEmailAddressLength).WithMessage($"Owner email must not exceed {Designly.Shared.Consts.MaxEmailAddressLength} characters.");
        }

    }
}
