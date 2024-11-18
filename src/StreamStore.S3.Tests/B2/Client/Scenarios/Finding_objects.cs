using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using FluentAssertions;
using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Finding_objects : Scenario<B2S3ClientSuite>
    {

        [Fact]
        public async Task When_finding_object_descriptor()
        {
            // Arrange
            var client = Suite.CreateB2S3Client();
            string key = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;
            var name = Generated.String;
            var files = new Mock<IStorageFiles>();
            Suite.B2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = Suite.MockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = key,
                        FileId = Generated.String
                    }
                }
            });

            files.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListFileVersionRequest>(r => r.Prefix == key),
                    It.IsAny<TimeSpan>()))
             .ReturnsAsync(apiResults.Object);

            // Act
            var result = await client.FindObjectDescriptorAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Key.Should().Be(key);
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_finding_object()
        {
            // Arrange
            var client = Suite.CreateB2S3Client();
            string key = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;

            var apiResults = Suite.MockRepository.Create<IApiResults<DownloadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new DownloadFileResponse());
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            Suite.B2Client.Setup(
                m => m.DownloadAsync(Suite.Settings.BucketName, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();

            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_getting_list_of_objects()
        {
            // Arrange
            var client = Suite.CreateB2S3Client();
            string sourcePrefix = Generated.String;
            string? startObjectName = Generated.String;
            CancellationToken token = default;
            var files = new Mock<IStorageFiles>();
            Suite.B2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = Suite.MockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = startObjectName,
                        FileId = Generated.String
                    }
                }
            });

            files.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListFileVersionRequest>(r =>
                        r.BucketId == Suite.Settings.BucketId
                        && r.Prefix == sourcePrefix
                        && r.StartFileId == startObjectName),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await client.ListObjectsAsync(
                sourcePrefix,
                startObjectName,
                token);

            // Assert
            result.Should().NotBeNull();
            result!.Objects.Should().NotBeNull();
            result!.Objects!.Should().HaveCount(1);
            result!.Objects![0].Key.Should().Be(startObjectName);
            Suite.MockRepository.VerifyAll();
        }
    }
}
