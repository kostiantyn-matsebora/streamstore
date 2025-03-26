using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Creating : Scenario<S3StreamContextSuite>
    {
        [Fact]
        public void When_creating()
        {
            // Arrange
            var revision = Generated.Primitives.Revision;
            var streamId = Generated.Primitives.Id;

            // Act
            var streamContext = Suite.CreateStreamContext(streamId, revision);

            // Assert
            streamContext.StreamId.Should().Be(streamId);
            streamContext.ExpectedRevision.Should().Be(revision);
            streamContext.Transient.Should().BeEquivalentTo(Suite.Transient);
            streamContext.Persistent.Should().BeEquivalentTo(Suite.Persistent);
        }
    }
}
