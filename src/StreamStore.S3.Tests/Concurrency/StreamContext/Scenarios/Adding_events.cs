using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Adding_events : Scenario<S3StreamContextSuite>
    {
        [Fact]
        public async Task When_adding_transient_event()
        {
            // Arrange
            var streamId = Generated.Id;
            var revision = Generated.Revision;
            var streamContext = Suite.CreateStreamContext(streamId, revision);
            var record = Generated.EventRecords(count: 1).First();
            Suite.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            Suite.MockClient.Setup(x => x.UploadObjectAsync(It.IsAny<UploadObjectRequest>(), default))
                            .ReturnsAsync(new UploadObjectResponse() { Key = Generated.String, VersionId = Generated.String });
            // Act
            await streamContext.AddTransientEventAsync(record, default);

            // Assert
            streamContext.Transient.Events.Should().NotBeNullOrEmpty();
        }
    }
}
