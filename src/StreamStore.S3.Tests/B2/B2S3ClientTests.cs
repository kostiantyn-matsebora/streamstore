using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.B2
{
    public class B2SClientTests
    {
        MockRepository mockRepository;

        Mock<IStorageClient> b2Client;
        B2StreamDatabaseSettings settings;
        Mock<IStorageFiles> files;
        public B2SClientTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            b2Client = mockRepository.Create<IStorageClient>();

            var configurator =
                new B2DatabaseConfigurator(new ServiceCollection());

            settings = configurator
                .WithBucketId(GeneratedValues.String)
                .WithBucketName(GeneratedValues.String)
                .WithCredentials(GeneratedValues.String, GeneratedValues.String)
                .Build();
        }

        B2S3Client CreateB2S3Client()
        {
            return new B2S3Client(
                 settings,
                b2Client.Object);
        }

        [Fact]
        public async Task CopyByFileIdAsync_Should_InvokeCopying()
        {
            // Arrange
            var client = CreateB2S3Client();
            string sourceFileId = GeneratedValues.String;
            string sourceName = GeneratedValues.String;
            string destinationName = GeneratedValues.String;
            var files = new Mock<IStorageFiles>();
            b2Client.SetupGet(m => m.Files).Returns(files.Object);


            CancellationToken token = default;

            files.Setup(m => m.CopyAsync(It.IsAny<CopyFileRequest>()));

            // Act
            await client.CopyByFileIdAsync(
                sourceFileId,
                sourceName,
                destinationName,
                token);

            // Assert
            mockRepository.VerifyAll();
            files.VerifyAll();
        }

        [Fact]
        public async Task DeleteObjectByFileIdAsync_Should_InvokeDeleting()
        {
            // Arrange
            var client = CreateB2S3Client();
            string fileId = GeneratedValues.String;
            string key = GeneratedValues.String;
            string destinationName = GeneratedValues.String;
            var files = new Mock<IStorageFiles>();
            b2Client.SetupGet(m => m.Files).Returns(files.Object);


            CancellationToken token = default;

           files.Setup(m => m.DeleteAsync(fileId, key));

            // Act
            await client.DeleteObjectByFileIdAsync(fileId, key, token);

            // Assert
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FindObjectDescriptorAsync_Should_FindObjectDescriptor()
        {
            // Arrange
            var client = CreateB2S3Client();
            string key = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;
            var name = GeneratedValues.String;
            var files = new Mock<IStorageFiles>();
            b2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = mockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = key,
                        FileId = GeneratedValues.String
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
            result!.FileName.Should().Be(key);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FindObjectAsync_Should_FindObject()
        {
            // Arrange
            var client = CreateB2S3Client();
            string key = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;

            var apiResults = mockRepository.Create<IApiResults<DownloadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new DownloadFileResponse());
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            b2Client.Setup(
                m => m.DownloadAsync(settings.BucketName, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();

            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ListObjectsAsync_Should_ReturnObjects()
        {
            // Arrange
            var client = CreateB2S3Client();
            string sourcePrefix = GeneratedValues.String;
            string? startObjectName = GeneratedValues.String;
            CancellationToken token = default;
            var files = new Mock<IStorageFiles>();
            b2Client.SetupGet(m => m.Files).Returns(files.Object);

            var apiResults = mockRepository.Create<IApiResults<ListFileVersionResponse>>();
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);
            apiResults.SetupGet(m => m.Response).Returns(new ListFileVersionResponse 
            {
                Files = new List<FileItem>
                {
                    new FileItem
                    {
                        FileName = startObjectName,
                        FileId = GeneratedValues.String
                    }
                }
            });

            files.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListFileVersionRequest>(r =>
                        (r.BucketId == settings.BucketId)
                        .And(r.Prefix == sourcePrefix)
                        .And(r.StartFileId == startObjectName)),
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
            result!.Objects![0].FileName.Should().Be(startObjectName);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task UploadObjectAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var aWSS3Client = CreateB2S3Client();
            string key = GeneratedValues.String;
            string fileId = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            var apiResults = mockRepository.Create<IApiResults<UploadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new UploadFileResponse() {  FileName = key, FileId = fileId });
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            b2Client
                .Setup(m => m.UploadAsync(settings.BucketId, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
