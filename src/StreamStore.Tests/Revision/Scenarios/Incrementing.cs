using FluentAssertions;
using StreamStore.Testing;


namespace StreamStore.Tests.RevisionObject
{
    public class Incrementing: Scenario<RevisionTestEnvironment>
    {

        [Fact]
        public void When_revision_is_incremented()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision();

            // Act
            var result = revision.Next();

            // Assert
            result.Should().NotBe(revision);
            result.Value.Should().Be(revision.Value + 1);
        }
    }
}
