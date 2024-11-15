using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;
using Converter = StreamStore.Serialization.Converter;


namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Getting_metadata: Scenario<S3StreamContextSuite>
    {
        [Fact]
        public async Task When_persistent_metadata_does_not_exist()
        {
            // Arrange
            var streamId = Generated.Id;
            var revision = Generated.Revision;
            var streamContext = Suite.CreateStreamContext(streamId, revision);
            Suite.MockClient.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default)).ReturnsAsync((FindObjectResponse?)null);
            Suite.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            var metadata = await streamContext.GetPersistentMetadataAsync(default);

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().BeEmpty();

        }

        [Fact]
        public async Task When_persistent_metadata_exists()
        {
            // Arrange
            var streamId = Generated.Id;
            var revision = Generated.Revision;
            var streamContext = Suite.CreateStreamContext(streamId, revision);
            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new[] { new EventMetadataRecord { Id = Generated.Id, Revision = Generated.Revision, Timestamp = Generated.DateTime } }),
                Key = Generated.String,
                VersionId = Generated.String
            };

            Suite.MockClient.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default)).ReturnsAsync(response);
            Suite.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            var metadata = await streamContext.GetPersistentMetadataAsync(default);

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().NotBeEmpty();

        }
    }
}
