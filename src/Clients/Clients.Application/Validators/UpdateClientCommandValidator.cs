using Clients.Application.Commands;
using Clients.Domain.ValueObjects;
using FluentValidation;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(command => command.client)
            .NotNull()
            .WithMessage("The submitted client object is not valid or empty");

        // Address and City in Address are required
        RuleFor(command => command.client.Address.City)
            .NotNull()
            .Must(city => !string.IsNullOrEmpty(city))
            .WithMessage($"Invalid value for {nameof(Address.City)}");
        
        // ContactDetails and PrimaryPhoneNumber in ContactDetails are required
        RuleFor(command => command.client.ContactDetails.PrimaryPhoneNumber)
            .NotNull()
            .Must(primaryPhoneNumber => !string.IsNullOrEmpty(primaryPhoneNumber))
            .WithMessage($"Invalid value for {nameof(ContactDetails.PrimaryPhoneNumber)}");
    }
}