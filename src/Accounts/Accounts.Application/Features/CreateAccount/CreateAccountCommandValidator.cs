using Accounts.Domain;
using FluentValidation;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.OwnerFirstName).NotEmpty().WithMessage("Owner first name is required.");
            RuleFor(p => p.OwnerFirstName).MaximumLength(Consts.FirstNameMaxLength).WithMessage("Owner first name must not exceed 100 characters.");

            RuleFor(p => p.OwnerLastName).NotEmpty().WithMessage("Owner last name is required.");
            RuleFor(p => p.OwnerLastName).MaximumLength(Consts.LastNameMaxLength).WithMessage("Owner last name must not exceed 100 characters.");

            RuleFor(p => p.OwnerEmail).NotEmpty().WithMessage("Owner email is required.");
            RuleFor(p => p.OwnerEmail).MaximumLength(Consts.EmailAddressMaxLength).WithMessage("Owner email must not exceed 100 characters.");
        }

    }
}
