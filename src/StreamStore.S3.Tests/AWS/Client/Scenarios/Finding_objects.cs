using Amazon.S3.Model;
using Moq;
using StreamStore.Testing;
using FluentAssertions;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Finding_objects : Scenario<AWSS3ClientTestEnvironment>
    {

        [Fact]
        public async Task When_finding_object_descriptors()
        {
            // Arrange
            var client = Environment.Client;
            string key = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;
            var name = Generated.Primitives.String;

            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = key } }
            };

            Environment.AmazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r => r.Prefix == key),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await client.FindObjectDescriptorAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Key.Should().Be(key);
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_finding_objects()
        {
            // Arrange
            var client = Environment.Client;
            string key = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;
            var response = new GetObjectResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                VersionId = Generated.Primitives.String,
                ResponseStream = new MemoryStream(content)
            };

            Environment.AmazonClient.Setup(
                m => m.GetObjectAsync(
                            It.Is<GetObjectRequest>(r =>
                                 r.BucketName == Environment.Settings.BucketName
                                 && r.Key == key),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Data.Should().BeEquivalentTo(content);
            result!.VersionId.Should().Be(response.VersionId);

            Environment.MockRepository.VerifyAll();
        }


        [Fact]
        public async Task When_getting_list_of_objects()
        {
            // Arrange
            var client = Environment.Client;
            string sourcePrefix = Generated.Primitives.String;
            string? startObjectName = Generated.Primitives.String;
            CancellationToken token = default;

            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = startObjectName } }
            };

            Environment.AmazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r =>
                        r.BucketName == Environment.Settings.BucketName
                        && r.Prefix == sourcePrefix
                        && r.KeyMarker == startObjectName),
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
            result!.Objects![0].Key.Should().Be(startObjectName);
            Environment.MockRepository.VerifyAll();
        }
    }
}
