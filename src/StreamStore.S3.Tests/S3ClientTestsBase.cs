using System.Collections.Concurrent;
using AutoFixture;
using Bytewizer.Backblaze.Extensions;
using FluentAssertions;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests
{

    public abstract class S3ClientTestsBase
    {
        readonly IS3Factory? factory;
        readonly IS3Client? client;

        public S3ClientTestsBase(IS3Factory? factory)
        {
            this.factory = factory;
            client = this.factory?.CreateClient();
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldNotFindFileDoesNotExist()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            // Act
            var file = await client!.FindObjectAsync(RandomString, CancellationToken.None);

            // Assert
            file.Should().BeNull();
        }

        [SkippableFact]
        public async Task UploadObjectAsync_ShouldUploadAndDeleteFileAsync()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var data = RandomByteArray;
            var objectName = RandomString;
            string? fileId = null;

            try
            {
                // Act
                fileId = await UploadObject(data, objectName);

                // Assert
                fileId.Should().NotBeNull();
                await AssertObjectIsFoundAndValid(objectName, data, fileId);
            }
            finally
            {
                //// Cleanup && Assert
                if (fileId != null)
                    await DeleteObjectAndAssert(objectName, fileId);
            }
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldFindObject()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var data = RandomByteArray;
            var objectName = RandomString;
            string? fileId = null;

            try
            {
                // Arrange
                fileId = await UploadObject(data, objectName);

                // Act && Assert
                await AssertObjectIsFoundAndValid(objectName, data, fileId);
            }
            finally
            {
                // Cleanup && Assert
                if (fileId != null)
                    await DeleteObjectAndAssert(objectName, fileId);
            }
        }

        async Task DeleteObjectAndAssert(string objectName, string fileId)
        {
            var act = () => client!.DeleteObjectByFileIdAsync(fileId, objectName, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }

        async Task<string?> UploadObject(byte[] data, string objectName)
        {
            var result = await client!.UploadObjectAsync(new UploadObjectRequest
            {
                Key = objectName,
                Data = data
            }, CancellationToken.None);
            return result?.FileId;
        }

        async Task AssertObjectIsFoundAndValid(string name, byte[] data, string? fileId = null)
        {
            var file = await client!.FindObjectAsync(name, CancellationToken.None);
            file.Should().NotBeNull();
            file!.FileId
                .Should().NotBeNullOrEmpty()
                .And.BeEquivalentTo(fileId ?? file.FileId);
            file!.Data.Should().BeEquivalentTo(data);
        }

        static string RandomString => new Fixture().Create<string>();

        static byte[] RandomByteArray => Converter.ToByteArray(RandomString);
    }
}
