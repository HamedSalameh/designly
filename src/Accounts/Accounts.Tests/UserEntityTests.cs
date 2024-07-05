using Accounts.Domain;
using static Accounts.Domain.Consts;

namespace Accounts.Tests
{
    [TestFixture]
    public class UserEntityTests
    {
        private readonly string accountName = "test_account";
        private readonly string userFirstName = "test_user_fn";
        private readonly string userLastName = "test_user_ln";
        private readonly string userEmail = "test_user_email@server.com";
        private readonly string userJobTitle = "test_user_job_title";

        [Test]
        public void User_CreatedWithAllProperties_HasCorrectValues()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            Assert.That(user.FirstName, Is.EqualTo(userFirstName));
            Assert.That(user.LastName, Is.EqualTo(userLastName));
            Assert.That(user.Email, Is.EqualTo(userEmail));
            Assert.That(user.JobTitle, Is.EqualTo(userJobTitle));
            Assert.That(user.Status, Is.EqualTo(UserStatus.BeforeActivation));
            Assert.That(user.Teams, Is.Empty);
            Assert.That(user.Account, Is.EqualTo(account));
            Assert.That(user.AccountId, Is.EqualTo(account.Id));
        }

        [Test]
        public void User_CreatedWithoutJobTitle_HasNullJobTitle()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            Assert.That(user.JobTitle, Is.Null);
        }

        [Test]
        public void User_ToString_ReturnsCorrectValue()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            Assert.That(user.ToString(), Is.EqualTo($"{userFirstName} {userLastName} ({userEmail})"));
        }

        [Test]
        public void User_Activate_WhenBeforeActivation_ChangesStatusToActive()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            user.Activate();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void User_Activate_WhenActive_DoesNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();

            user.Activate();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void User_Activate_WhenSuspended_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();
            user.Suspend();

            Assert.Throws<InvalidOperationException>(() => user.Activate());
        }

        [Test]
        public void User_Suspend_WhenActive_ChangesStatusToSuspended()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();

            user.Suspend();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Suspended));
        }

        [Test]
        public void User_Suspend_WhenSuspended_DoesNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();
            user.Suspend();

            user.Suspend();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Suspended));
        }

        [Test]
        public void User_Suspend_WhenBeforeActivation_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            Assert.Throws<InvalidOperationException>(() => user.Suspend());
        }

        [Test]
        public void User_Disable_WhenBeforeActivation_ChangesStatusToDisabled()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            user.Disable();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void User_Disable_WhenActive_ChangesStatusToDisabled()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();

            user.Disable();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void User_Disable_WhenSuspended_ChangesStatusToDisabled()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();
            user.Suspend();

            user.Disable();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void User_Disable_WhenDisabled_DoesNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Disable();

            user.Disable();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Disabled));
        }

        [Test]
        public void User_Disable_WhenBeforeActivation_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Status = UserStatus.Deleted;
            Assert.Throws<InvalidOperationException>(() => user.Disable());
        }

        [Test]
        public void User_MarkForDeletion_WhenMarkedForDeletion_DoesNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Status = UserStatus.MarkedForDeletion;

            user.MarkForDeletion();

            Assert.That(user.Status, Is.EqualTo(UserStatus.MarkedForDeletion));
        }

        [Test]
        public void User_MarkForDeletion_WhenActive_ChangesStatusToMarkedForDeletion()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();

            user.MarkForDeletion();

            Assert.That(user.Status, Is.EqualTo(UserStatus.MarkedForDeletion));
        }

        [Test]
        public void User_MarkForDeletion_WhenSuspended_ChangesStatusToMarkedForDeletion()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();
            user.Suspend();

            user.MarkForDeletion();

            Assert.That(user.Status, Is.EqualTo(UserStatus.MarkedForDeletion));
        }

        [Test]
        public void User_MarkForDeletion_WhenDeleted_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Status = UserStatus.Deleted;

            Assert.Throws<InvalidOperationException>(() => user.MarkForDeletion());
        }

        // delete user test
        [Test]
        public void User_MarkDeleted_WhenDeleted_DoesNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Status = UserStatus.Deleted;

            user.MarkDeleted();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Deleted));
        }

        [Test]
        public void User_MarkDeleted_WhenMarkedForDeletion_ChangesStatusToDeleted()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.MarkForDeletion();

            user.MarkDeleted();

            Assert.That(user.Status, Is.EqualTo(UserStatus.Deleted));
        }

        [Test]
        public void User_MarkDeleted_WhenActive_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();

            Assert.Throws<InvalidOperationException>(() => user.MarkDeleted());
        }

        [Test]
        public void User_MarkDeleted_WhenSuspended_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Activate();
            user.Suspend();

            Assert.Throws<InvalidOperationException>(() => user.MarkDeleted());
        }

        [Test]
        public void User_MarkDeleted_WhenBeforeActivation_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);

            Assert.Throws<InvalidOperationException>(() => user.MarkDeleted());
        }

        [Test]
        public void User_MarkDeleted_WhenDisabled_ThrowsInvalidOperationException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, userJobTitle, account);
            user.Disable();

            Assert.Throws<InvalidOperationException>(() => user.MarkDeleted());
        }

    }
}
