using FluentAssertions;
using StreamStore.Testing;


namespace StreamStore.Tests.RevisionObject
{
    public class Incrementing: Scenario<RevisionTestSuite>
    {

        [Fact]
        public void When_revision_is_incremented()
        {
            // Arrange
            var revision = RevisionTestSuite.CreateRevision();

            // Act
            var result = revision.Increment();

            // Assert
            result.Should().NotBe(revision);
            result.Value.Should().Be(revision.Value + 1);
        }
    }
}
