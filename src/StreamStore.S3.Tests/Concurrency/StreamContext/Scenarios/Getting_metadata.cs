using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;
using Converter = StreamStore.Serialization.Converter;


namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Getting_metadata: Scenario<S3StreamContextTestEnvironment>
    {
        [Fact]
        public async Task When_persistent_metadata_does_not_exist()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            var revision = Generated.Primitives.Revision;
            var streamContext = Environment.CreateStreamContext(streamId, revision);
            Environment.MockClient.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default)).ReturnsAsync((FindObjectResponse?)null);
            Environment.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

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
            var streamId = Generated.Primitives.Id;
            var revision = Generated.Primitives.Revision;
            var streamContext = Environment.CreateStreamContext(streamId, revision);
            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new[] { new EventMetadataRecord { Id = Generated.Primitives.Id, Revision = Generated.Primitives.Revision, Timestamp = Generated.Primitives.DateTime } }),
                Key = Generated.Primitives.String,
                VersionId = Generated.Primitives.String
            };

            Environment.MockClient.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default)).ReturnsAsync(response);
            Environment.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            var metadata = await streamContext.GetPersistentMetadataAsync(default);

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().NotBeEmpty();

        }
    }
}
