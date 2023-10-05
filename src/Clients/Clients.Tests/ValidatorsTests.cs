using Clients.Application.Commands;
using Clients.Application.Validators;
using Clients.Domain.Entities;
using Clients.Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Clients.Tests
{
    public class ValidatorsTests
    {
        private const string FirstName = "John";
        private const string FamilyName = "Doe";
        private const string City = "Utopia";
        private const string PrimaryPhoneNumber = "0542123123";
        private readonly Guid TenantId = Guid.NewGuid();
        
        [Test]
        public void CreateClientCommandValidator_ShouldPass()
        {
            // Arrange
            var validator = new CreateClientCommandValidator();
            
            var validCommand = new CreateClientCommand(
                new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId));
            
            // Act
            var result = validator.TestValidate(validCommand);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Test]
        public void CreateClientCommandValidator_ShouldFail_MissingPrimaryPhoneNumber()
        {
            // Arrange
            var validator = new CreateClientCommandValidator();
            
            var invalidCommand = new CreateClientCommand(
                new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId));
            
            // inject invalid values
            invalidCommand.Client.ContactDetails.PrimaryPhoneNumber = string.Empty;
            
            // Act
            var result = validator.TestValidate(invalidCommand);
            
            // Assert
            result.ShouldHaveValidationErrorFor(command => command.Client.ContactDetails.PrimaryPhoneNumber);
        }
        
        [Test]
        public void CreateClientCommandValidator_ShouldFail_MissingCity()
        {
            // Arrange
            var validator = new CreateClientCommandValidator();
            
            var invalidCommand = new CreateClientCommand(
                new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId));
            
            // inject invalid values
            invalidCommand.Client.Address.City = string.Empty;
            
            // Act
            var result = validator.TestValidate(invalidCommand);
            
            // Assert
            result.ShouldHaveValidationErrorFor(command => command.Client.Address.City);
        }
    }
}