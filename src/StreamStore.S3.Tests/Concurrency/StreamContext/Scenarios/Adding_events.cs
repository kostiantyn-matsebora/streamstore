using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Adding_events : Scenario<S3StreamContextTestEnvironment>
    {
        [Fact]
        public async Task When_adding_transient_event()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            var revision = Generated.Primitives.Revision;
            var streamContext = Environment.CreateStreamContext(streamId, revision);
            var record = Generated.StreamEventRecords.Many(count: 1).First();
            Environment.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            Environment.MockClient.Setup(x => x.UploadObjectAsync(It.IsAny<UploadObjectRequest>(), default))
                            .ReturnsAsync(new UploadObjectResponse() { Key = Generated.Primitives.String, VersionId = Generated.Primitives.String });
            // Act
            await streamContext.AddTransientEventAsync(record, default);

            // Assert
            streamContext.Transient.Events.Should().NotBeNullOrEmpty();
        }
    }
}
