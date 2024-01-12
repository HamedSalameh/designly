

using Accounts.Domain;

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

        [Test]
        public void CreateUser_WithValidParameters_ShouldCreateUser()
        {
            // Arrange
            // Act
            var user = new User(FirstName, LastName, Email, JobTitle, MemberOf);

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
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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
            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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

            Assert.Throws<ArgumentNullException>(() => new User(firstName, lastName, email, jobTitle, memberOf));
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
            var user = new User(firstName, lastName, email, jobTitle, memberOf);

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
            var user = new User(firstName, lastName, email, jobTitle, memberOf);

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
            var user = new User(firstName, lastName, email, jobTitle, memberOf);

            // Assert
            Assert.That(user.FirstName, Is.EqualTo(FirstName));
            Assert.That(user.LastName, Is.EqualTo(LastName));
            Assert.That(user.Email, Is.EqualTo(Email));
            Assert.That(user.JobTitle, Is.EqualTo(JobTitle));
            Assert.That(user.MemberOf, Is.EqualTo(MemberOf));
        }
    }
}
