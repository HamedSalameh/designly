using Accounts.Domain;
using static Accounts.Domain.Consts;

namespace Accounts.Tests
{
    [TestFixture]
    public class TeamEntityTests
    {
        private Guid teamId = Guid.NewGuid();

        private readonly string accountName = "test_account";
        private readonly string userFirstName = "test_user_fn";
        private readonly string userLastName = "test_user_ln";
        private readonly string userEmail = "test_user_email@server.com";
        private readonly string userJobTitle = "test_user_job_title";

        private Account account;

        [SetUp]
        public void Setup()
        {
            account = new Account(accountName);
            account.Id = Guid.NewGuid();
        }

        [Test]
        public void Team_Constructor_WithValidArguments_ShouldCreateTeam()
        {
            // Arrange
            string teamName = "Test Team";

            // Act
            Team team = new Team(teamName, account);

            // Assert
            Assert.That(team, Is.Not.Null);
            Assert.That(team.Name, Is.EqualTo(teamName));
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(0));
            Assert.That(team.Status, Is.EqualTo(TeamStatus.Active));
            Assert.That(team.Account, Is.EqualTo(account));
            Assert.That(team.AccountId, Is.EqualTo(account.Id));
        }

        [Test]
        public void Team_AddMember_WithValidArguments_ShouldAddMember()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            // Act
            team.AddMember(user);

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(1));
            Assert.That(team.Members.First(), Is.EqualTo(user));
        }

        [Test]
        public void Team_AddMember_WithExistingMember_ShouldNotAddMember()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            // Act
            team.AddMember(user);
            team.AddMember(user);

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(1));
            Assert.That(team.Members.First(), Is.EqualTo(user));
        }

        [Test]
        public void Team_ToString_ShouldReturnTeamNameAndMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMember(user);

            // Act
            string result = team.ToString();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo($"Team: Test Team{Environment.NewLine}Members:{Environment.NewLine}{user.ToString()}{Environment.NewLine}"));
        }

        [Test]
        public void Team_RemoveMember_WithValidArguments_ShouldRemoveMember()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMember(user);

            // Act
            team.RemoveMember(user);

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(0));
        }

        [Test]
        public void Team_AddMembers_WithValidArguments_ShouldAddMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            // Act
            team.AddMembers(new List<User> { user1, user2 });

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(2));
            Assert.That(team.Members, Contains.Item(user1));
            Assert.That(team.Members, Contains.Item(user2));
        }

        [Test]
        public void Team_AddMembers_WithExistingMembers_ShouldNotAddMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            // Act
            team.AddMembers(new List<User> { user1, user2 });
            team.AddMembers(new List<User> { user1, user2 });

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(2));
            Assert.That(team.Members, Contains.Item(user1));
            Assert.That(team.Members, Contains.Item(user2));
        }

        [Test]
        public void Team_RemoveMembers_WithValidArguments_ShouldRemoveMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMembers(new List<User> { user1, user2 });

            // Act
            team.RemoveMembers(new List<User> { user1, user2 });

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(0));
        }

        [Test]
        public void Team_RemoveMembers_WithNonExistingMembers_ShouldNotRemoveMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user3 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMembers(new List<User> { user1, user2 });

            // Act
            team.RemoveMembers(new List<User> { user2, user3 });

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(1));
            Assert.That(team.Members, Contains.Item(user1));
            Assert.That(team.Members, !Contains.Item(user2));
        }

        [Test]
        public void Team_RemoveMembers_WithEmptyList_ShouldNotRemoveMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMembers(new List<User> { user1, user2 });

            // Act
            team.RemoveMembers(new List<User>());

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(2));
            Assert.That(team.Members, Contains.Item(user1));
            Assert.That(team.Members, Contains.Item(user2));
        }

        [Test]
        public void Team_RemoveMembers_WithNullList_ShouldNotRemoveMembers()
        {
            // Arrange
            Team team = new Team("Test Team", account);
            User user1 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            User user2 = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            team.AddMembers(new List<User> { user1, user2 });

            // Act
            Assert.Throws<ArgumentNullException>(() => team.RemoveMembers(null!));
        }

        [Test]
        public void Team_RemoveMember_WithNullUser_ShouldThrowArgumentNullException()
        {
            // Arrange
            Team team = new Team("Test Team", account);

            // Act
            Assert.Throws<ArgumentNullException>(() => team.RemoveMember(null!));
        }

        [Test]
        public void Team_AddMember_WithNullUser_ShouldThrowArgumentNullException()
        {
            // Arrange
            Team team = new Team("Test Team", account);

            // Act
            Assert.Throws<ArgumentNullException>(() => team.AddMember(null!));
        }

        [Test]
        public void Team_AddMembers_WithNullUsers_ShouldThrowArgumentNullException()
        {
            // Arrange
            Team team = new Team("Test Team", account);

            // Act
            Assert.Throws<ArgumentNullException>(() => team.AddMembers(null!));
        }

        [Test]
        public void Team_AddMembers_WithEmptyUsers_ShouldDoNothing()
        {
            // Arrange
            Team team = new Team("Test Team", account);

            // Act
            team.AddMembers(new List<User>());

            // Assert
            Assert.That(team.Members, Is.Not.Null);
            Assert.That(team.Members.Count, Is.EqualTo(0));
        }

        [Test]
        public void Team_ToString_WithNoMembers_ShouldReturnTeamName()
        {
            // Arrange
            Team team = new Team("Test Team", account);

            // Act
            string result = team.ToString();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo($"Team: Test Team{Environment.NewLine}Members:{Environment.NewLine}"));
        }

    }
}
