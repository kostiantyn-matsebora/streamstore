using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Serialization;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.StreamContainer
{
    public class Loading_stream : Scenario<S3StorageTestEnvironment>
    {

        [Fact]
        public async Task When_stream_does_not_exist()
        {
            // Arrange
            var streamContainer = Environment.CreateS3StreamContainer();
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            Environment.MockS3Client.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default))
                              .ReturnsAsync(default(FindObjectResponse));

            // Act
            await streamContainer.LoadAsync(startFrom: Revision.One, count: 1);

            // Assert
            streamContainer.State.Should().Be(S3ObjectState.DoesNotExist);
        }

        [Fact]
        public async Task When_start_from_greater_than_revision()
        {
            // Arrange
            var streamContainer = Environment.CreateS3StreamContainer();
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            var events = new EventMetadataRecordCollection(new[]
            {
                new EventMetadataRecord { Id = "1", Revision = Revision.One }
            });
            Environment.MockS3Client.Setup(x => x.FindObjectAsync(It.IsAny<string>(), default))
                              .ReturnsAsync(new FindObjectResponse { Data = Converter.ToByteArray(events.ToArray()) });

            // Act
            await streamContainer.LoadAsync(startFrom: 2, count: 1);

            // Assert
            streamContainer.State.Should().Be(S3ObjectState.Loaded);
            streamContainer.Events.Should().HaveCount(0);
        }

        [Fact]
        public async Task When_start_less_or_equal_revision()
        {
            // Arrange
            var streamContainer = Environment.CreateS3StreamContainer();
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            var metadata = new EventMetadataRecordCollection(new[]
            {
                new EventMetadataRecord { Id = "1", Revision = Revision.One },
                new EventMetadataRecord { Id = "2", Revision = 2 }
            });

            var @event = new EventRecord { Id = "2", Revision = 2, Data = Generated.Objects.ByteArray };

            Environment.MockS3Client
                .SetupSequence(x => x.FindObjectAsync(It.IsAny<string>(), default))
                .ReturnsAsync(new FindObjectResponse { Data = Converter.ToByteArray(metadata.ToArray()) })
                .ReturnsAsync(new FindObjectResponse { Data = Converter.ToByteArray(@event) });

            // Act
            await streamContainer.LoadAsync(startFrom: 2, count: 1);

            // Assert
            streamContainer.State.Should().Be(S3ObjectState.Loaded);
            streamContainer.Events.Should().HaveCount(1);
            streamContainer.Events.First().Event!.Id.Should().Be("2");
            streamContainer.Events.First().Event!.Revision.Should().Be(2);
        }
    }
}
