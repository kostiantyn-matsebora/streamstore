using FluentAssertions;
using StreamStore.S3.Client;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests
{

    public abstract class S3ClientIntegrationTestsBase<TSuite>: TestsBase<TSuite> where TSuite : IS3Suite
    {
        readonly IS3Client? client;

        protected S3ClientIntegrationTestsBase(TSuite suite): base(suite)
        {
            if (suite.ArePrerequisitiesMet)
                client = suite.CreateClientFactory()!.CreateClient();
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldNotFindFileDoesNotExist()
        {
            TrySkip();

            // Arrange
            // Act
            var file = await client!.FindObjectAsync(GeneratedValues.String, CancellationToken.None);

            // Assert
            file.Should().BeNull();
        }

        [SkippableFact]
        public async Task UploadObjectAsync_ShouldUploadAndDeleteFileAsync()
        {
            TrySkip();

            // Arrange
            var data = GeneratedValues.ByteArray;
            var objectName = GeneratedValues.String;
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
                // Cleanup && Assert
                if (response != null)
                    await DeleteObjectAndAssert(objectName, response.FileId!);
            }
        }

        [SkippableFact]
        public async Task FindObjectAsync_ShouldFindObject()
        {
            TrySkip();

            // Arrange
            var data = GeneratedValues.ByteArray;
            var objectName = GeneratedValues.String;
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
    }
}
