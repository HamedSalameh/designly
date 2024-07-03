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

        // unit tests for AccountEntity

        [Test]
        public void AccountEntity_WithoutOwner_ShouldBeCreated()
        {
            var account = new Account(accountName);

            Assert.That(account.Name, Is.EqualTo(accountName));
        }

        [Test]
        public void AccountEntity_WithOwner_ShouldBeCreated()
        {
            var user = new User(userFirstName, userLastName, userEmail, new Account(accountName));
            var account = new Account(accountName, user);

            Assert.Multiple(() =>
            {
                Assert.That(account.Name, Is.EqualTo(accountName));
                Assert.That(account.Owner, Is.EqualTo(user));
            });
        }

        [Test]
        public void AccountEntity_WithNullOwner_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(accountName, null!));
        }

        [Test]
        public void AccountEntity_WithEmptyName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(string.Empty));
        }

        [Test]
        public void AccountEntity_WithWhiteSpaceName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(" "));
        }

        [Test]
        public void AccountEntity_WhenCreated_ShouldHaveInProcessRegisterationStatus()
        {
            var account = new Account(accountName);

            Assert.That(account.Status, Is.EqualTo(AccountStatus.InProcessRegisteration));
        }

        [Test]
        public void AccountEntity_WhenOwnerAssigned_ShouldHaveOwner()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);

            Assert.That(account.Owner, Is.EqualTo(user));
        }

        [Test]
        public void AccountEntity_WhenOwnerAssignedWithNull_ShouldThrowArgumentNullException()
        {
            var account = new Account(accountName);

            Assert.Throws<ArgumentNullException>(() => account.AssignOwner(null!));
        }

        [Test]
        public void AccountEntity_WhenDefaultTeamCreated_ShouldHaveDefaultTeam()
        {
            var account = new Account(accountName);

            account.CreateDefaultTeam();

            Assert.That(account.Teams, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
        }

        [Test]
        public void AccountEntity_WhenDefaultTeamCreatedTwice_ShouldHaveOneDefaultTeam()
        {
            var account = new Account(accountName);

            account.CreateDefaultTeam();
            account.CreateDefaultTeam();

            Assert.That(account.Teams, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
        }

        [Test]
        public void AccountEntity_WhenUserAddedToDefaultTeam_ShouldHaveUserInDefaultTeam()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.CreateDefaultTeam();
            account.AddUserToDefaultTeam(user);

            Assert.That(account.Teams.First().Members, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First().Members.First(), Is.EqualTo(user));
        }

        [Test]
        public void AccountEntity_WhenUserAddedToDefaultTeamTwice_ShouldHaveUserInDefaultTeamOnce()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.CreateDefaultTeam();
            account.AddUserToDefaultTeam(user);
            account.AddUserToDefaultTeam(user);

            Assert.That(account.Teams.First().Members, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First().Members.First(), Is.EqualTo(user));
        }

        // add, remove teams methods unit testing
        [Test]
        public void AccountEntity_WhenTeamAdded_ShouldHaveTeam()
        {
            var account = new Account(accountName);
            var team = new Team("test_team", account);

            account.AddTeam(team);

            Assert.That(account.Teams, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First(), Is.EqualTo(team));
        }

        [Test]
        public void AccountEntity_WhenTeamAddedTwice_ShouldHaveTeamOnce()
        {
            var account = new Account(accountName);
            var team = new Team("test_team", account);

            account.AddTeam(team);
            account.AddTeam(team);

            Assert.That(account.Teams, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First(), Is.EqualTo(team));
        }

        [Test]
        public void AccountEntity_WhenTeamAddedWithNull_ShouldThrowArgumentNullException()
        {
            var account = new Account(accountName);

            Assert.Throws<ArgumentNullException>(() => account.AddTeam(null!));
        }

        [Test]
        public void AccountEntity_WhenTeamRemoved_ShouldNotHaveTeam()
        {
            var account = new Account(accountName);
            var team = new Team("test_team", account);

            account.AddTeam(team);
            account.RemoveTeam(team);

            Assert.That(account.Teams, Has.Count.EqualTo(0));
        }

        [Test]
        public void AccountEntity_WhenTeamRemovedTwice_ShouldNotThrowException()
        {
            var account = new Account(accountName);
            var team = new Team("test_team", account);

            account.AddTeam(team);
            account.RemoveTeam(team);
            account.RemoveTeam(team);

            Assert.That(account.Teams, Has.Count.EqualTo(0));
        }

        [Test]
        public void AccountEntity_WhenTeamRemovedWithNull_ShouldThrowArgumentNullException()
        {
            var account = new Account(accountName);

            Assert.Throws<ArgumentNullException>(() => account.RemoveTeam(null!));
        }


        // Activate Account unit tests
        [Test]
        public void AccountEntity_WhenActivated_WithNoOwner_ShouldThrowAccountException()
        {
            var account = new Account(accountName);

            Assert.Throws<AccountException>(() => account.ActivateAccount());
        }

        [Test]
        public void AccountEntity_WhenActivated_WithTransientAccount_ShouldThrowAccountException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);

            Assert.Throws<AccountException>(() => account.ActivateAccount());
        }

        [Test]
        public void AccountEntity_WhenActivated_ShouldHaveActiveStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            Assert.That(account.Status, Is.EqualTo(AccountStatus.Active));
        }

        [Test]
        public void AccountEntity_WhenActivated_ShouldHaveDefaultTeam()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            Assert.That(account.Teams, Has.Count.EqualTo(1));
            Assert.That(account.Teams.First().Name, Is.EqualTo(DefaultTeamName));
        }

        [Test]
        public void AccountEntity_WhenActivated_WithMarkedAsDeleted_ShouldThrowAccountException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.MarkAccountForDeletion();

            Assert.Throws<AccountException>(() => account.ActivateAccount());
        }

        [Test]
        public void AccountEntity_WhenActivated_WithActiveStatus_ShouldNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.ActivateAccount();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.Active));
        }

        // Suspended Account unit tests
        [Test]
        public void AccountEntity_WhenSuspended_ShouldHaveSuspendedStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.SuspendAccount();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.Suspended));
        }

        [Test]
        public void AccountEntity_WhenSuspended_WithMarkedAsDeleted_ShouldThrowAccountException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.MarkAccountForDeletion();

            Assert.Throws<AccountException>(() => account.SuspendAccount());
        }

        [Test]
        public void AccountEntity_WhenSuspended_WithSuspendedStatus_ShouldNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.SuspendAccount();
            account.SuspendAccount();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.Suspended));
        }

        // Disable Account unit tests
        [Test]
        public void AccountEntity_WhenDisabled_ShouldHaveDisabledStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.DisableAccount();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.Disabled));
        }

        [Test]
        public void AccountEntity_WhenDisabled_WithMarkedAsDeleted_ShouldThrowAccountException()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.MarkAccountForDeletion();

            Assert.Throws<AccountException>(() => account.DisableAccount());
        }

        [Test]
        public void AccountEntity_WhenDisabled_WithDisabledStatus_ShouldNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.ActivateAccount();
            account.DisableAccount();
            account.DisableAccount();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.Disabled));
        }

        // Mark Account For Deletion unit tests
        [Test]
        public void AccountEntity_WhenMarkedForDeletion_ShouldHaveMarkedForDeletionStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.MarkAccountForDeletion();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.MarkedForDeletion));
        }

        [Test]
        public void AccountEntity_WhenMarkedForDeletion_WithMarkedForDeletionStatus_ShouldNotChangeStatus()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);
            account.CreateDefaultTeam();
            
            // Simulate account is saved and already has an Id
            account.Id = Guid.NewGuid();

            account.MarkAccountForDeletion();
            account.MarkAccountForDeletion();

            Assert.That(account.Status, Is.EqualTo(AccountStatus.MarkedForDeletion));
        }

        // ToString method unit tests
        [Test]
        public void AccountEntity_ToString_ShouldReturnAccountName()
        {
            var account = new Account(accountName);

            Assert.That(account.ToString(), Is.EqualTo(accountName + ", " + Enum.GetName( AccountStatus.InProcessRegisteration)));
        }

        [Test]
        public void AccountEntity_ToString_WithOwner_ShouldReturnAccountNameAndOwnerName()
        {
            var account = new Account(accountName);
            var user = new User(userFirstName, userLastName, userEmail, account);

            account.AssignOwner(user);

            Assert.That(account.ToString(), Is.EqualTo(accountName + ", " + user.ToString() + ", " +  Enum.GetName(AccountStatus.InProcessRegisteration)));
        }

        // unit test Equals method
        [Test]
        public void AccountEntity_Equals_WithNull_ShouldReturnFalse()
        {
            var account = new Account(accountName);

            Assert.That(account.Equals(null), Is.False);
        }

        [Test]
        public void AccountEntity_Equals_WithDifferentType_ShouldReturnFalse()
        {
            var account = new Account(accountName);

            Assert.That(account.Equals(new object()), Is.False);
        }

        [Test]
        public void AccountEntity_Equals_WithDifferentAccount_ShouldReturnFalse()
        {
            var account = new Account(accountName);
            var account2 = new Account("test_account2");

            Assert.That(account.Equals(account2), Is.False);
        }

        [Test]
        public void AccountEntity_Equals_WithSameAccount_ShouldReturnTrue()
        {
            var account = new Account(accountName);

            Assert.That(account.Equals(account), Is.True);
        }

        [Test]
        public void AccountEntity_Equals_WithTransientAccount_ShouldReturnFalse()
        {
            var account = new Account(accountName);
            var account2 = new Account(accountName);

            Assert.That(account.Equals(account2), Is.False);
        }

        [Test]
        public void AccountEntity_Equals_WithSameAccountId_ShouldReturnTrue()
        {
            var account = new Account(accountName);
            var account2 = new Account(accountName);

            account.Id = Guid.NewGuid();
            account2.Id = account.Id;

            Assert.That(account.Equals(account2), Is.True);
        }

        [Test]
        public void AccountEntity_Equals_WithDifferentAccountId_ShouldReturnFalse()
        {
            var account = new Account(accountName);
            var account2 = new Account(accountName);

            account.Id = Guid.NewGuid();
            account2.Id = Guid.NewGuid();

            Assert.That(account.Equals(account2), Is.False);
        }

    }
}
