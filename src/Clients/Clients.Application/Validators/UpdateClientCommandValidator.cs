using Clients.Application.Commands;
using FluentValidation;

namespace Clients.Application.Validators
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(command => command.Client)
                .NotNull()
                .WithMessage("The submitted client object is not valid or empty");
        }
    }
}