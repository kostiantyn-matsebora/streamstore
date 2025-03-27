using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using FluentAssertions;
using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Finding_objects : Scenario<B2S3ClientTestEnvironment>
    {

        [Fact]
        public async Task When_finding_object_descriptor()
        {
            // Arrange
            var client = Environment.CreateB2S3Client();
            string key = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;
            var name = Generated.Primitives.String;
            var files = new Mock<IStorageFiles>();
            Environment.B2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = Environment.MockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = key,
                        FileId = Generated.Primitives.String
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
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_finding_object()
        {
            // Arrange
            var client = Environment.CreateB2S3Client();
            string key = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;

            var apiResults = Environment.MockRepository.Create<IApiResults<DownloadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new DownloadFileResponse());
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            Environment.B2Client.Setup(
                m => m.DownloadAsync(Environment.Settings.BucketName, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();

            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_getting_list_of_objects()
        {
            // Arrange
            var client = Environment.CreateB2S3Client();
            string sourcePrefix = Generated.Primitives.String;
            string? startObjectName = Generated.Primitives.String;
            CancellationToken token = default;
            var files = new Mock<IStorageFiles>();
            Environment.B2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = Environment.MockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = startObjectName,
                        FileId = Generated.Primitives.String
                    }
                }
            });

            files.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListFileVersionRequest>(r =>
                        r.BucketId == Environment.Settings.BucketId
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
            Environment.MockRepository.VerifyAll();
        }
    }
}
