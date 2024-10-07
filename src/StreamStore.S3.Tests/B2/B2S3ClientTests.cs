using AutoFixture;
using FluentAssertions;
using StreamStore.S3.B2;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests.B2
{
    public class B2S3ClientTests
    {
        B2S3Factory? factory;
        public B2S3ClientTests()
        {
            factory = B2TestsSuite.CreateFactory();
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldNotFindFileDoesNotExist()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var client = factory!.CreateClient();
            var fixture = new Fixture();

            // Act
            var file = await client.FindObjectAsync(fixture.Create<string>(), CancellationToken.None);

            // Assert
            file.Should().BeNull();
        }

        [SkippableFact]
        public async Task UploadObjectAsync_ShouldUploadAndDeleteFileAsync()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var client = factory!.CreateClient();
            var fixture = new Fixture();
            var data = Converter.ToByteArray(fixture.Create<string>());
            var objectName = fixture.Create<string>();
            string? fileId = null;
            try
            {
                // Act
                var response = await client.UploadObjectAsync(new UploadObjectRequest
                {
                    Key = objectName,
                    Data = data
                }, CancellationToken.None);

                fileId = response?.FileId;

                // Assert
                response.Should().NotBeNull();
                response!.FileId.Should().NotBeNullOrEmpty();
            }
            finally
            {
                // Cleanup && Assert
                if (fileId != null)
                {
                    var act = () => client.DeleteObjectAsync(objectName, CancellationToken.None, fileId);
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldFindObject()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var client = factory!.CreateClient();
            var fixture = new Fixture();
            var data = Converter.ToByteArray(fixture.Create<string>());
            var objectName = fixture.Create<string>();
            string? fileId = null;
            try
            {
                // Arrange
                var response = await client.UploadObjectAsync(new UploadObjectRequest
                {
                    Key = objectName,
                    Data = data
                }, CancellationToken.None);

                fileId = response?.FileId;

                // Act
                var file = await client.FindObjectAsync(objectName, CancellationToken.None);

                // Assert
                response.Should().NotBeNull();
                response!.FileId.Should().NotBeNullOrEmpty();
                file!.Data.Should().BeEquivalentTo(data);

            }
            finally
            {
                // Cleanup && Assert
                if (fileId != null)
                {
                    var act = () => client.DeleteObjectAsync(objectName, CancellationToken.None, fileId);
                    await act.Should().NotThrowAsync();
                }
            }
        }
    }
}
