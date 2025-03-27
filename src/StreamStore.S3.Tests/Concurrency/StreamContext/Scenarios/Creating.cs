using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Creating : Scenario<S3StreamContextTestEnvironment>
    {
        [Fact]
        public void When_creating()
        {
            // Arrange
            var revision = Generated.Primitives.Revision;
            var streamId = Generated.Primitives.Id;

            // Act
            var streamContext = Environment.CreateStreamContext(streamId, revision);

            // Assert
            streamContext.StreamId.Should().Be(streamId);
            streamContext.ExpectedRevision.Should().Be(revision);
            streamContext.Transient.Should().BeEquivalentTo(Environment.Transient);
            streamContext.Persistent.Should().BeEquivalentTo(Environment.Persistent);
        }
    }
}
