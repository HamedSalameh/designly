using Accounts.Domain;
using FluentValidation;


namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandValidator : AbstractValidator<ValidateUserCommand>
    {
        public ValidateUserCommandValidator()
        {
            RuleFor(cmd => cmd.userId)
                .NotEmpty();

            RuleFor(cmd => cmd.tenantId)
                .NotEmpty();
        }
    }
}
