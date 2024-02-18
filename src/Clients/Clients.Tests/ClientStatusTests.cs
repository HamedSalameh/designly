using Clients.Domain;

namespace Clients.Tests
{
    [TestFixture]
    public class ClientStatusTests
    {
        [Test]
        public void NonExistent_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.NonExistent;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.NonExistent));
                Assert.That(status.Description, Is.EqualTo("Non-existent"));
            });
        }

        [Test]
        public void Active_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.Active;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.Active));
                Assert.That(status.Description, Is.EqualTo("Active"));
            });
        }

        [Test]
        public void Inactive_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.Inactive;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.Inactive));
                Assert.That(status.Description, Is.EqualTo("Inactive"));
            });
        }

        [Test]
        public void Suspended_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.Suspended;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.Suspended));
                Assert.That(status.Description, Is.EqualTo("Suspended"));
            });
        }

        [Test]
        public void HighRisk_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.HighRisk;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.HighRisk));
                Assert.That(status.Description, Is.EqualTo("High Risk"));
            });
        }

        [Test]
        public void Blacklisted_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.Blacklisted;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.Blacklisted));
                Assert.That(status.Description, Is.EqualTo("Blacklisted"));
            });
        }

        [Test]
        public void Unsupported_StatusPropertiesMatch()
        {
            // Arrange & Act
            var status = ClientStatus.Unsupported;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(status.Code, Is.EqualTo(ClientStatusCode.Unsupported));
                Assert.That(status.Description, Is.EqualTo("Unsupported"));
            });
        }
    }
}
