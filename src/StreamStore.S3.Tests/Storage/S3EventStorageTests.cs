using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Serialization;
using StreamStore.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StreamStore.S3.Tests.Storage
{
    public class S3EventStorageTests
    {
        readonly MockRepository mockRepository;
        readonly Mock<IS3Client> mockS3Client;
        readonly Mock<IS3ClientFactory> mockS3ClientFactory;
        readonly S3ContainerPath path;

        public S3EventStorageTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockS3Client = new Mock<IS3Client>(MockBehavior.Strict);
            this.mockS3ClientFactory = this.mockRepository.Create<IS3ClientFactory>();
            this.path = new S3ContainerPath(Generated.String);

            mockS3ClientFactory
                .Setup(x => x.CreateClient())
                .Returns(mockS3Client.Object);
        }

        S3EventStorage CreateS3EventStorage()
        {
            return new S3EventStorage(path, mockS3ClientFactory.Object);
        }

        [Fact]
        public async Task AppendAsync_Should_AddAndUploadEvent()
        {
            // Arrange
            var s3EventStorage = this.CreateS3EventStorage();
            var fixture = new Fixture();
            EventRecord record = fixture.Create<EventRecord>();
            CancellationToken token = default;
            mockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
          
            mockS3Client
                .Setup(x => x.UploadObjectAsync(It.Is<UploadObjectRequest>(r => r.Key!.Contains(record.Id) && r.Key.Contains(path)), token))
                .ReturnsAsync(fixture.Create<UploadObjectResponse>());

            // Act
            await s3EventStorage.AppendAsync(record,token);

            // Assert
            s3EventStorage.NotEmpty.Should().BeTrue();
            s3EventStorage.Count().Should().Be(1);
            s3EventStorage.First().Event.Should().Be(record);
            s3EventStorage.Last().Event.Should().Be(record);

            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task LoadEventAsync_ShouldLoadEvent()
        {
            // Arrange
            var s3EventStorage = this.CreateS3EventStorage();
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

            mockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            mockS3Client
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

            this.mockRepository.VerifyAll();
        }
    }
}
