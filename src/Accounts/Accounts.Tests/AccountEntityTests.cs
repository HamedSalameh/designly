using Accounts.Domain;
using static Accounts.Domain.Consts;

namespace Accounts.Tests
{

    [TestFixture]
    public class AccountEntityTests
    {
        private readonly string accountName = "test_account";
        private readonly string userFirstName = "test_user_fn";
        private readonly string userLastName = "test_user_ln";
        private readonly string userEmail = "email@mail.com";
        

        [Test]
        public void CreateAccount_WithNullName_ThrowsArgumentNullException()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => new Account(null!));
        }

        [Test]
        public void CreateAccount_WithEmptyName_ThrowsArgumentNullException()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => new Account(string.Empty));
        }

        [Test]
        public void CreateAccount_WithNullOwner_ThrowsArgumentNullException()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => new Account("Test", null!));
        }

        [Test]
        public void CreateAccount_WithValidNameAndOwner_CreatesAccountWithStatusInProcessRegisteration()
        {
            // Arrange
            
            // Act
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(account.Name, Is.EqualTo(accountName));
                Assert.That(account.Status, Is.EqualTo(AccountStatus.InProcessRegisteration));
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void AssignOwner_WithNullOwner_ThrowsArgumentNullException()
        {
            // Arrange
            var account = new Account(accountName);
            Assert.Throws<ArgumentNullException>(() => account.AssignOwner(null!));
        }

        [Test]
        public void AssignOwner_WithValidOwner_AssignsOwner()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            // Act
            account.AssignOwner(accountOwner);

            // Assert
            Assert.That(account.Owner, Is.EqualTo(accountOwner));
        }

        [Test]
        public void CreateDefaultTeam_WithNullTeams_CreatesDefaultTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            // Act
            account.CreateDefaultTeam();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
            });
        }

        [Test]
        public void CreateDefaultTeam_WithEmptyTeams_CreatesDefaultTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            // Act
            account.CreateDefaultTeam();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
            });
        }

        [Test]
        public void CreateDefaultTeam_WithDefaultTeam_CreatesDefaultTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            account.CreateDefaultTeam();

            // Act
            account.CreateDefaultTeam();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
            });
        }

        [Test]
        public void AddUserToDefaultTeam_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            account.CreateDefaultTeam();

            // Act
            Assert.Throws<ArgumentNullException>(() => account.AddUserToDefaultTeam(null!));
        }

        [Test]
        public void AddUserToDefaultTeam_WithValidUser_AddsUserToDefaultTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            account.CreateDefaultTeam();
            var user = new User(userFirstName, userLastName, userEmail, account);

            // Act
            account.AddUserToDefaultTeam(user);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
                Assert.That(account.Teams.First().Members, Is.Not.Null);
                Assert.That(account.Teams.First().Members, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First().Members.First(), Is.EqualTo(user));
            });
        }

        [Test]
        public void AddTeam_WithNullTeam_ThrowsArgumentNullException()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            // Act
            Assert.Throws<ArgumentNullException>(() => account.AddTeam(null!));
        }

        [Test]
        public void AddTeam_WithValidTeam_AddsTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            var team = new Team("test_team", account);

            // Act
            account.AddTeam(team);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First(), Is.EqualTo(team));
            });
        }

        [Test]
        public void RemoveTeam_WithNullTeam_ThrowsArgumentNullException()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);

            // Act
            Assert.Throws<ArgumentNullException>(() => account.RemoveTeam(null!));
        }

        [Test]
        public void RemoveTeam_WithValidTeam_RemovesTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            var team = new Team("test_team", account);
            account.AddTeam(team);

            // Act
            account.RemoveTeam(team);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void RemoveTeam_WithValidTeamAndOtherTeams_RemovesTeam()
        {
            // Arrange
            var account = new Account(accountName);
            var accountOwner = new User(userFirstName, userLastName, userEmail, account);
            var team = new Team("test_team", account);
            var team2 = new Team("test_team2", account);
            account.AddTeam(team);
            account.AddTeam(team2);

            // Act
            account.RemoveTeam(team);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Teams, Is.Not.Null);
                Assert.That(account.Teams, Has.Count.EqualTo(1));
                Assert.That(account.Teams.First(), Is.EqualTo(team2));
            });
        }

        [Test]
        public void RemoveTeam_WithValidTeamAndNoTeams_ThrowsException()
        {
            // Arrange
            var account = new Account(accountName);

            // Act
            Assert.That(account.Teams, Is.Not.Null);
            Assert.That(account.Teams, Has.Count.EqualTo(0));
        }       
    }
}
