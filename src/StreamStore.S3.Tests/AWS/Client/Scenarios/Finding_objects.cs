using Amazon.S3.Model;
using Moq;
using StreamStore.Testing;
using FluentAssertions;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Finding_objects : Scenario<AWSS3ClientSuite>
    {

        [Fact]
        public async Task When_finding_object_descriptors()
        {
            // Arrange
            var client = Suite.Client;
            string key = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;
            var name = Generated.String;

            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = key } }
            };

            Suite.AmazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r => r.Prefix == key),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await client.FindObjectDescriptorAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Key.Should().Be(key);
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_finding_objects()
        {
            // Arrange
            var client = Suite.Client;
            string key = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;
            var response = new GetObjectResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                VersionId = Generated.String,
                ResponseStream = new MemoryStream(content)
            };

            Suite.AmazonClient.Setup(
                m => m.GetObjectAsync(
                            It.Is<GetObjectRequest>(r =>
                                 r.BucketName == Suite.Settings.BucketName
                                 && r.Key == key),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await client.FindObjectAsync(key, token);

            // Assert
            result.Should().NotBeNull();
            result!.Data.Should().BeEquivalentTo(content);
            result!.VersionId.Should().Be(response.VersionId);

            Suite.MockRepository.VerifyAll();
        }


        [Fact]
        public async Task When_getting_list_of_objects()
        {
            // Arrange
            var client = Suite.Client;
            string sourcePrefix = Generated.String;
            string? startObjectName = Generated.String;
            CancellationToken token = default;

            var response = new ListVersionsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Versions = new List<S3ObjectVersion> { new S3ObjectVersion { Key = startObjectName } }
            };

            Suite.AmazonClient.Setup(
                m => m.ListVersionsAsync(
                    It.Is<ListVersionsRequest>(r =>
                        r.BucketName == Suite.Settings.BucketName
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
            Suite.MockRepository.VerifyAll();
        }
    }
}
