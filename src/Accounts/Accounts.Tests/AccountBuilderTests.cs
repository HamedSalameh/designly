using Accounts.Application.Builders;
using Accounts.Domain;
using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Accounts.Tests
{
    [TestFixture]
    public class AccountBuilderTests
    {
        private ILogger<AccountBuilder> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<AccountBuilder>>();
        }

        [Test]
        public void CreateBasicAccount_ValidAccountName_AccountCreatedSuccessfully()
        {
            // Arrange
            var accountBuilder = new AccountBuilder(_logger);
            const string accountName = "ValidAccount";

            // Act
            var result = accountBuilder.CreateBasicAccount(accountName).Build();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(accountName));
        }

        [TestCase("")]
        [TestCase("    ")]
        public void CreateBasicAccount_InvalidAccountName_ThrowsArgumentException(string invalidAccountName)
        {
            // Arrange
            var accountBuilder = new AccountBuilder(_logger);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => accountBuilder.CreateBasicAccount(invalidAccountName));
        }

        [Test]
        public void Build_AccountNotInitialized_ThrowsBusinessLogicException()
        {
            // Arrange
            var accountBuilder = new AccountBuilder(_logger);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => accountBuilder.Build());
        }

        [Test]
        public void ConfigureAccount_AccountNotCreatedYet_ThrowsBusinessLogicException()
        {
            // Arrange
            var accountBuilder = new AccountBuilder(_logger);
            var account = new Account("test");
            var user = new User("John", "dor", "mail@server.com", account);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => accountBuilder.ConfigureAccount(user));
        }

        [Test]
        public void ConfigureAccount_ValidUser_AccountConfiguredSuccessfully()
        {
            // Arrange
            var accountBuilder = new AccountBuilder(_logger);
            var account = new Account("test");
            var user = new User("John", "dor", "mail@server.com", account);
            var accountOwner = user;

            // Act
            var result = accountBuilder.CreateBasicAccount("ValidAccount")
                                        .ConfigureAccount(accountOwner)
                                        .Build();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Owner, Is.Not.Null);
                Assert.That(result.Owner, Is.EqualTo(accountOwner));
                Assert.That(result.Teams, Has.Count.GreaterThan(0));
                Assert.That(result.Teams.First().Members.Contains(accountOwner), Is.True);
            });
            
        }
    }
}
