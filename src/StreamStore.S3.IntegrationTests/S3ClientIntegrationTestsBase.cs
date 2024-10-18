using System.Collections.Concurrent;
using AutoFixture;
using Bytewizer.Backblaze.Extensions;
using Bytewizer.Backblaze.Models;
using FluentAssertions;
using StreamStore.S3.Client;
using StreamStore.Serialization;

namespace StreamStore.S3.IntegrationTests
{

    public abstract class S3ClientIntegrationTestsBase
    {
        readonly IS3ClientFactory? factory;
        readonly IS3Client? client;

        protected S3ClientIntegrationTestsBase(IS3ClientFactory? factory)
        {
            this.factory = factory;
            client = this.factory?.CreateClient();
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldNotFindFileDoesNotExist()
        {
            Skip.IfNot(factory != null, "Database configuration is missing");

            // Arrange
            // Act
            var file = await client!.FindObjectAsync(RandomString, CancellationToken.None);

            // Assert
            file.Should().BeNull();
        }

        [SkippableFact]
        public async Task UploadObjectAsync_ShouldUploadAndDeleteFileAsync()
        {
            Skip.IfNot(factory != null, "Database configuration is missing");

            // Arrange
            var data = RandomByteArray;
            var objectName = RandomString;
            UploadObjectResponse? response = null;

            try
            {
                // Act
                response = await UploadObject(data, objectName);

                // Assert
                response.Should().NotBeNull();

                await AssertObjectIsFoundAndValid(response!, data);
            }
            finally
            {
                //// Cleanup && Assert
                if (response != null)
                    await DeleteObjectAndAssert(objectName, response.FileId!);
            }
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldFindObject()
        {
            Skip.IfNot(factory != null, "Database configuration is missing");

            // Arrange
            var data = RandomByteArray;
            var objectName = RandomString;
            UploadObjectResponse? response = null;

            try
            {
                // Arrange
                response = await UploadObject(data, objectName);

                // Act && Assert
                await AssertObjectIsFoundAndValid(response!, data);
            }
            finally
            {
                // Cleanup && Assert
                if (response != null)
                    await DeleteObjectAndAssert(objectName, response.FileId!);
            }
        }

        async Task DeleteObjectAndAssert(string objectName, string fileId)
        {
            var act = () => client!.DeleteObjectByFileIdAsync(fileId, objectName, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }

        async Task<UploadObjectResponse?> UploadObject(byte[] data, string objectName)
        {
            return await client!.UploadObjectAsync(new UploadObjectRequest
            {
                Key = objectName,
                Data = data
            }, CancellationToken.None);
        }

        async Task AssertObjectIsFoundAndValid(UploadObjectResponse response, byte[] data)
        {
            var file = await client!.FindObjectAsync(response.Name!, CancellationToken.None);
            file.Should().NotBeNull();
            file!.Name.Should().BeEquivalentTo(response.Name);
            if (file!.FileId is not null)
                file.FileId.Should().BeEquivalentTo(response.FileId);
            file!.Data.Should().BeEquivalentTo(data);
        }

        static string RandomString => new Fixture().Create<string>();

        static byte[] RandomByteArray => Converter.ToByteArray(RandomString);
    }
}
