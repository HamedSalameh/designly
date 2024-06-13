using Clients.Application.Commands;
using Clients.Application.Validators;
using Clients.Domain.Entities;
using Designly.Shared.ValueObjects;
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
       
    }
}