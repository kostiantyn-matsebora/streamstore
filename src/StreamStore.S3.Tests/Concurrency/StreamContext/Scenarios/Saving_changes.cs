using Bytewizer.Backblaze.Models;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class Saving_changes : Scenario<S3StreamContextSuite>
    {

        [Fact]
        public async Task When_saving_changes()
        {

            // Arrange
            var streamId = Generated.Primitives.Id;
            var revision = Generated.Primitives.Revision;
            var streamContext = Suite.CreateStreamContext(streamId, revision);
            Suite.MockClient.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            var record = Generated.Many(count: 1).First();
            Suite.MockClient.Setup(x => x.UploadObjectAsync(It.IsAny<UploadObjectRequest>(), default))
                            .ReturnsAsync(new UploadObjectResponse() { Key = Generated.Primitives.String, VersionId = Generated.Primitives.String });
            Suite.MockClient.SetupSequence(x => x.FindObjectDescriptorAsync(It.IsAny<string>(), default))
                            .ReturnsAsync(new ObjectDescriptor { Key = Generated.Primitives.String, VersionId = Generated.Primitives.String })
                            .ReturnsAsync(new ObjectDescriptor { Key = Generated.Primitives.String, VersionId = Generated.Primitives.String })
                            .ReturnsAsync((ObjectDescriptor?)null);
            Suite.MockClient.Setup(x => x.CopyByVersionIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                            .Returns(Task.CompletedTask);
            Suite.MockClient.Setup(x => x.DeleteObjectByVersionIdAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                            .Returns(Task.CompletedTask);
            Suite.MockClient.Setup(x => x.ListObjectsAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                            .ReturnsAsync(new ListS3ObjectsResponse { Objects = Array.Empty<ObjectDescriptor>() });
            await streamContext.AddTransientEventAsync(record, default);

            // Act
            await streamContext.SaveChangesAsync(default);

            // Assert
            streamContext.Persistent.Events.Should().NotBeEmpty();
        }
    }
}
