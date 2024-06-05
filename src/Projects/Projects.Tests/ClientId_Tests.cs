using Projects.Domain.StonglyTyped;

namespace Projects.Tests
{
    [TestFixture]
    public class ClientId_Tests
    {
        // unit test for strong typed ProjectId

        [Test]
        public void CompareTo_ShouldReturnZero_WhenIdsAreEqual()
        {
            // Arrange
            var id1 = new ClientId(Guid.NewGuid());
            var id2 = new ClientId(id1.Id);

            // Act
            var result = id1.CompareTo(id2);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CompareTo_ShouldReturnZegative_WhenIdsNotEqual()
        {
            // Arrange
            var id1 = new ClientId(Guid.NewGuid());
            var id2 = new ClientId(Guid.NewGuid());

            // Act
            var result = id1.CompareTo(id2);

            // Assert
            Assert.That(result, Is.Not.EqualTo(0));
        }

        [Test]
        public void OperatorEquals_ShouldReturnTrue_WhenIdsAreEqual()
        {
            // Arrange
            var id1 = new ClientId(Guid.NewGuid());
            var id2 = new ClientId(id1.Id);

            // Act
            var result = id1 == id2;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CompareTo_ShouldReturnNegative_WhenFirstIdIsLess()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));

            Assert.That(taskItemId1.CompareTo(taskItemId2), Is.LessThan(0));
        }

        [Test]
        public void OperatorLessThan_ShouldReturnTrue_WhenFirstIdIsLess()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));

            var result = taskItemId1 < taskItemId2;

            Assert.That(result, Is.True);
        }

        [Test]
        public void OperatorLessThan_ShouldReturnFalse_WhenFirstIdIsGreater()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));

            var result = taskItemId1 < taskItemId2;

            Assert.That(result, Is.False);
        }

        [Test]
        // Testing <= operator
        public void OperatorLessThanOrEqual_ShouldReturnTrue_WhenFirstIdIsLess()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));

            var result = taskItemId1 <= taskItemId2;

            Assert.That(result, Is.True);
        }

        [Test]
        // Testing <= operator
        public void OperatorLessThanOrEqual_ShouldReturnTrue_WhenFirstIdIsEqual()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));

            var result = taskItemId1 <= taskItemId2;

            Assert.That(result, Is.False);
        }

        [Test]
        public void OperatorGreaterThanOrEqual_ShouldReturnTrue_WhenFirstIdIsEqual()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));

            var result = taskItemId1 >= taskItemId2;

            Assert.That(result, Is.True);
        }

        [Test]
        public void OperatorGreaterThan_ShouldReturnTrue_WhenFirstIdIsEqual()
        {
            var taskItemId1 = new ClientId(new Guid("12345678-1234-1234-1234-123456789013"));
            var taskItemId2 = new ClientId(new Guid("12345678-1234-1234-1234-123456789012"));

            var result = taskItemId1 > taskItemId2;

            Assert.That(result, Is.True);
        }
    }
}
