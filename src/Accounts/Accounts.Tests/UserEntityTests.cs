

using Accounts.Domain;
using static Accounts.Domain.Consts;

namespace Accounts.Tests
{
    [TestFixture]
    public class UserEntityTests
    {
        public string FirstName = "firstName";
        public string LastName = "lastName";
        public string JobTitle = "jobTitle";
        public string Email = "email@mail.com";
        public Guid MemberOf = Guid.NewGuid();
        public readonly Guid TenantId = Guid.NewGuid();

        [Test]
        public void CreateUser_WithValidParameters_ShouldCreateUser()
        {
            // Arrange
            // Act
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);

            // Assert
            Assert.That(user.FirstName, Is.EqualTo(FirstName));
            Assert.That(user.LastName, Is.EqualTo(LastName));
            Assert.That(user.Email, Is.EqualTo(Email));
            Assert.That(user.JobTitle, Is.EqualTo(JobTitle));
            Assert.That(user.MemberOf, Is.EqualTo(MemberOf));
        }

        [Test]
        public void CreateUser_WithNullFirstName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = (string)null;
            var lastName = LastName;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            // Act
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithEmptyFirstName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = string.Empty;
            var lastName = LastName;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithNullLastName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = (string)null;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            // Act
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithEmptyLastName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = string.Empty;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithNullEmail_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = (string)null;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            // Act
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithEmptyEmail_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = string.Empty;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithDefaultMemberOf_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = Guid.Empty;

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithDefaultGuidMemberOf_ShouldThrowArgumentNullException()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = default(Guid);

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf, TenantId));
        }

        [Test]
        public void CreateUser_WithNullJobTitle_ShouldCreateUser()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = Email;
            var jobTitle = (string)null;
            var memberOf = MemberOf;

            // Act
            var user = new User(firstName, lastName, email, jobTitle, memberOf, TenantId);

            // Assert
            Assert.That(user.FirstName, Is.EqualTo(FirstName));
            Assert.That(user.LastName, Is.EqualTo(LastName));
            Assert.That(user.Email, Is.EqualTo(Email));
            Assert.That(user.JobTitle, Is.EqualTo(jobTitle));
            Assert.That(user.MemberOf, Is.EqualTo(MemberOf));
        }

        [Test]
        public void CreateUser_WithEmptyJobTitle_ShouldCreateUser()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = Email;
            var jobTitle = string.Empty;
            var memberOf = MemberOf;

            // Act
            var user = new User(firstName, lastName, email, jobTitle, memberOf, TenantId);

            // Assert
            Assert.That(user.FirstName, Is.EqualTo(FirstName));
            Assert.That(user.LastName, Is.EqualTo(LastName));
            Assert.That(user.Email, Is.EqualTo(Email));
            Assert.That(user.JobTitle, Is.EqualTo(jobTitle));
            Assert.That(user.MemberOf, Is.EqualTo(MemberOf));
        }

        [Test]
        public void CreateUser_WithDefaultJobTitle_ShouldCreateUser()
        {
            // Arrange
            var firstName = FirstName;
            var lastName = LastName;
            var email = Email;
            var jobTitle = JobTitle;
            var memberOf = MemberOf;

            // Act
            var user = new User(firstName, lastName, email, jobTitle, memberOf, TenantId);

            // Assert
            Assert.That(user.FirstName, Is.EqualTo(FirstName));
            Assert.That(user.LastName, Is.EqualTo(LastName));
            Assert.That(user.Email, Is.EqualTo(Email));
            Assert.That(user.JobTitle, Is.EqualTo(JobTitle));
            Assert.That(user.MemberOf, Is.EqualTo(MemberOf));
        }

        // Testing UserStatus operations
        [Test]
        public void ActivateUser_WithUserStatusBeforeActivation_ShouldActivateUser()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);

            // Act
            user.Activate();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void ActivateUser_WithUserStatusActive_ShouldNotChangeUserStatus()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();

            // Act
            user.Activate();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void ActivateUser_WithUserStatusSuspended_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();
            user.Suspend();

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Activate());
        }

        [Test]
        public void ActivateUser_WithUserStatusDisabled_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();
            user.Suspend();
            user.Disable();

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Activate());
        }

        [Test]
        public void ActivateUser_WithUserStatusMarkedForDeletion_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();
            user.Suspend();
            user.Disable();
            user.MarkForDeletion();

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Activate());
        }

        [Test]
        public void SuspendUser_WithUserStatusActive_ShouldSuspendUser()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();

            // Act
            user.Suspend();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Suspended));
        }

        [Test]
        public void SuspendUser_WithUserStatusSuspended_ShouldNotChangeUserStatus()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();
            user.Suspend();

            // Act
            user.Suspend();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Suspended));
        }

        [Test]
        public void SuspendUser_WithUserStatusBeforeActivation_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Suspend());
        }

        [Test]
        public void SuspendUser_WithUserStatusDisabled_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Disable();

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Suspend());
        }

        [Test]
        public void SuspendUser_WithUserStatusMarkedForDeletion_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.MarkForDeletion();

            // Act
            Assert.Throws<InvalidOperationException>(() => user.Suspend());
        }

        [Test]
        public void DisableUser_WithUserStatusBeforeActivation_ShouldDisableUser()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);

            // Act
            user.Disable();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void DisableUser_WithUserStatusActive_ShouldDisableUser()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();

            // Act
            user.Disable();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void DisableUser_WithUserStatusSuspended_ShouldDisableUser()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Activate();
            user.Suspend();

            // Act
            user.Disable();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void DisableUser_WithUserStatusDisabled_ShouldNotChangeUserStatus()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.Disable();

            // Act
            user.Disable();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void DisableUser_WithUserStatusMarkedForDeletion_ShouldNotChangeUserStatus()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.MarkForDeletion();

            // Assert
            Assert.Throws<InvalidOperationException>(() => user.Disable());
        }

        [Test]
        [TestCase(UserStatus.BeforeActivation)]
        [TestCase(UserStatus.Active)]
        [TestCase(UserStatus.Suspended)]
        [TestCase(UserStatus.Disabled)]
        [TestCase(UserStatus.MarkedForDeletion)]
        public void MarkForDeletionUser_ShouldMarkForDeletionUser(UserStatus initialStatus)
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);

            // Set initial user status
            SetUserStatus(user, initialStatus);

            // Act
            user.MarkForDeletion();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.MarkedForDeletion));
        }

        private static void SetUserStatus(User user, UserStatus status)
        {
            switch (status)
            {
                case UserStatus.BeforeActivation:
                    // No action needed as it's the default status.
                    break;
                case UserStatus.Active:
                    user.Activate();
                    break;
                case UserStatus.Suspended:
                    user.Activate();
                    user.Suspend();
                    break;
                case UserStatus.Disabled:
                    user.Disable();
                    break;
                case UserStatus.MarkedForDeletion:
                    user.MarkForDeletion();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        [Test]
        public void MarkDeleted_UserStatusMarkedForDeletion_ShouldMarkUserAsDeleted()
        {
            // Arrange
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf, TenantId);
            user.MarkForDeletion();

            // Act
            user.MarkDeleted();

            // Assert
            Assert.That(user.Status, Is.EqualTo(UserStatus.Deleted));
        }

    }
}
