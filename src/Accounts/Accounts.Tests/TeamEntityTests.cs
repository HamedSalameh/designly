
using Accounts.Domain;

namespace Accounts.Tests
{
    [TestFixture]
    public class TeamEntityTests
    {
        private readonly Guid TenantId = Guid.NewGuid();

        [Test]
        public void CreateTeam_WithValidParameters_ShouldCreateTeam()
        {
            // Arrange
            var teamName = "Test Team";
            var memberOf = Guid.NewGuid();

            // Act
            var team = new Team(teamName, memberOf, TenantId);

            // Assert
            Assert.That(team.Name, Is.EqualTo(teamName));
            Assert.That(team.MemberOf, Is.EqualTo(memberOf));
        }

        [Test]
        public void CreateTeam_WithNullName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var teamName = (string)null;
            var memberOf = Guid.NewGuid();

            // Act
            Assert.Throws<ArgumentNullException>(() => new Team(teamName, memberOf, TenantId));
        }

        [Test]
        public void CreateTeam_WithEmptyName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var teamName = string.Empty;
            var memberOf = Guid.NewGuid();

            Assert.Throws<ArgumentNullException>(() => new Team(teamName, memberOf, TenantId));
        }

        [Test]
        public void CreateTeam_WithDefaultMemberOf_ShouldThrowArgumentNullException()
        {
            // Arrange
            var teamName = "Test Team";
            var memberOf = Guid.Empty;

            Assert.Throws<ArgumentNullException>(() => new Team(teamName, memberOf, TenantId));
        }

        [Test]
        public void CreateTeam_WithDefaultGuidMemberOf_ShouldThrowArgumentNullException()
        {
            // Arrange
            var teamName = "Test Team";
            var memberOf = default(Guid);

            Assert.Throws<ArgumentNullException>(() => new Team(teamName, memberOf, TenantId));
        }
    }
}