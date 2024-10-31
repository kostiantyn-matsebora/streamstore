using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Serialization;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.EventStorage
{
    public class Loading_events: Scenario<S3StorageTestSuite>
    {
        [Fact]
        public async Task When_loading_events()
        {
            // Arrange
            var s3EventStorage = Suite.CreateS3EventStorage();
            Id eventId = Generated.Id;
            var fixture = new Fixture();
            CancellationToken token = default(global::System.Threading.CancellationToken);
            var record = fixture.Create<EventRecord>();
            var response = new FindObjectResponse
            {
                FileId = Generated.String,
                Name = Generated.String,
                Data = Converter.ToByteArray(record)
            };

            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            Suite.MockS3Client
                .Setup(x => x.FindObjectAsync(It.Is<string>(x => x.Contains(eventId)), token))
                .ReturnsAsync(response);

            // Act
            var result = await s3EventStorage.LoadEventAsync(
                eventId,
                token);

            // Assert
            s3EventStorage.NotEmpty.Should().BeTrue();
            s3EventStorage.Count().Should().Be(1);
            s3EventStorage.First().Event!.Id.Should().Be(record.Id);
            s3EventStorage.Last().Event!.Data.Should().BeEquivalentTo(record.Data);

            Suite.MockRepository.VerifyAll();
        }
    }
}
