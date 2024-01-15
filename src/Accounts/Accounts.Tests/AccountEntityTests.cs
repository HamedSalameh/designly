
using Accounts.Domain;

namespace Accounts.Tests
{
    [TestFixture]
    public class AccountEntityTests
    {
        [Test]
        public void CreateAccount_WithValidParameters_ShouldCreateAccount()
        {
            // Arrange
            var accountName = "Test Account";
            var accountOwner = Guid.NewGuid();

            // Act
            var account = new Account(accountName, accountOwner);

            // Assert
            Assert.That(account.Name, Is.EqualTo(accountName));
            Assert.That(account.AccountOwner, Is.EqualTo(accountOwner));
        }

        [Test]
        public void CreateAccount_WithNullName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var accountName = (string)null;
            var accountOwner = Guid.NewGuid();

            // Act
            Assert.Throws<ArgumentNullException>(() => new Account(accountName, accountOwner));
        }

        [Test]
        public void CreateAccount_WithEmptyName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var accountName = string.Empty;
            var accountOwner = Guid.NewGuid();

            Assert.Throws<ArgumentNullException>(() => new Account(accountName, accountOwner));
        }

        [Test]
        public void CreateAccount_WithDefaultAccountOwner_ShouldThrowArgumentNullException()
        {
            // Arrange
            var accountName = "Test Account";
            var accountOwner = Guid.Empty;

            Assert.Throws<ArgumentNullException>(() => new Account(accountName, accountOwner));
        }

        [Test]
        public void CreateAccount_WithDefaultGuidAccountOwner_ShouldThrowArgumentNullException()
        {
            // Arrange
            var accountName = "Test Account";
            var accountOwner = default(Guid);

            Assert.Throws<ArgumentNullException>(() => new Account(accountName, accountOwner));
        }
    }
}