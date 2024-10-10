using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Polly;
using StreamStore.S3.AWS;
using StreamStore.S3.Client;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3ClientTests
    {
        MockRepository mockRepository;

        Mock<IAmazonS3> amazonClient;
        AWSS3DatabaseSettings settings;

        public AWSS3ClientTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            amazonClient = mockRepository.Create<IAmazonS3>();
            var configurator =
                new AWSS3DatabaseConfigurator(new ServiceCollection());

            settings = configurator.Build();
        }

        AWSS3Client CreateAWSS3Client()
        {
            return new AWSS3Client(
                amazonClient.Object,
                settings);
        }

        [Fact]
        public async Task CopyByFileIdAsync_Should_InvokeCopying()
        {
            // Arrange
            var client = CreateAWSS3Client();
            string sourceFileId = GeneratedValues.String; // Provide a non-null value
            string sourceName = GeneratedValues.String; // Provide a non-null value
            string destinationName = GeneratedValues.String; // Provide a non-null value


            CancellationToken token = default;

            amazonClient.Setup(
                m => m.CopyObjectAsync(
                    It.Is<CopyObjectRequest>(r =>
                       (r.DestinationKey == destinationName)
                       .And(r.SourceKey == sourceName)
                       .And(r.SourceVersionId == sourceFileId)
                       .And(r.SourceBucket == settings.BucketName)
                       .And(r.DestinationBucket == settings.BucketName)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CopyObjectResponse());

            // Act
            await client.CopyByFileIdAsync(
                sourceFileId,
                sourceName,
                destinationName,
                token);

            // Assert
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteObjectByFileIdAsync_Should_InvokeDeleting()
        {
            // Arrange
            // Arrange
            var client = CreateAWSS3Client();
            string fileId = GeneratedValues.String; // Provide a non-null value
            string key = GeneratedValues.String; // Provide a non-null value
            string destinationName = GeneratedValues.String; // Provide a non-null value


            CancellationToken token = default;

            amazonClient.Setup(
                m => m.DeleteObjectAsync(
                    It.Is<DeleteObjectRequest>(r =>
                      (r.BucketName == settings.BucketName)
                      .And(r.Key == key)
                      .And(r.VersionId == fileId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteObjectResponse());

            // Act
            await client.DeleteObjectByFileIdAsync(fileId, key, token);

            // Assert
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FindObjectDescriptorAsync_Should_FindObjectDescriptor()
        {
            // Arrange
            var client = CreateAWSS3Client();
            string key = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;
            var name = GeneratedValues.String;
            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = key } }
            };
            It.Is<ListVersionsRequest>(r =>

                       (r.BucketName == settings.BucketName)
                       .And(r.Prefix == key)
                       );

            amazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r => r.Prefix == key),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

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
            var client = CreateAWSS3Client();
            string key = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;
            var response = new GetObjectResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                VersionId = GeneratedValues.String,
                ResponseStream = new MemoryStream(content)
            };

            amazonClient.Setup(
                m => m.GetObjectAsync(
                    It.Is<GetObjectRequest>(r =>
                        (r.BucketName == settings.BucketName)
                        .And(r.Key == key)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Data.Should().BeEquivalentTo(content);
            result!.FileId.Should().Be(response.VersionId);

            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ListObjectsAsync_Should_ReturnObjects()
        {
            // Arrange
            var client = CreateAWSS3Client();
            string sourcePrefix = GeneratedValues.String;
            string? startObjectName = GeneratedValues.String;
            CancellationToken token = default;

            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = startObjectName } }
            };

            amazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r =>
                        (r.BucketName == settings.BucketName)
                        .And(r.Prefix == sourcePrefix)
                        .And(r.KeyMarker == startObjectName)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

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
            var aWSS3Client = CreateAWSS3Client();
            string key = GeneratedValues.String;
            CancellationToken token = default;
            var content = GeneratedValues.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            amazonClient.Setup(m => m.PutObjectAsync(
                It.Is<PutObjectRequest>(r =>
                    (r.BucketName == settings.BucketName)
                    .And(r.Key == key)
                    .And(r.InputStream != null)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PutObjectResponse());

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DisposeAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var client = CreateAWSS3Client();

            amazonClient.Setup(m => m.Dispose());
            // Act
            await client.DisposeAsync();

            // Assert

            mockRepository.VerifyAll();
        }
    }
}
