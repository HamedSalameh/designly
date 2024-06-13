using Clients.Application.Commands;
using Designly.Shared.ValueObjects;
using FluentValidation;

namespace Clients.Application.Validators
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(command => command.Client)
                .NotNull()
                .WithMessage("The submitted client object is not valid or empty");
        }
    }
}