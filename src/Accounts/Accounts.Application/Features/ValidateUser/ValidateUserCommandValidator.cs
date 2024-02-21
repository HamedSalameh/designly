using Accounts.Domain;
using FluentValidation;


namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandValidator : AbstractValidator<ValidateUserCommand>
    {
        public ValidateUserCommandValidator()
        {
            RuleFor(cmd => cmd.UserId)
                .NotEmpty();

            RuleFor(cmd => cmd.TenantId)
                .NotEmpty();
        }
    }
}
