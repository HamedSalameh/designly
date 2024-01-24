using Accounts.Domain;
using FluentValidation;


namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandValidator : AbstractValidator<ValidateUserCommand>
    {
        public ValidateUserCommandValidator()
        {
            RuleFor(cmd => cmd.Email)
                .NotEmpty();

            RuleFor(cmd => cmd.tenantId)
                .NotEmpty();
        }
    }
}
