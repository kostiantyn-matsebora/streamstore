using System.Collections.Concurrent;
using AutoFixture;
using Bytewizer.Backblaze.Extensions;
using FluentAssertions;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests
{

    public abstract class S3ClientTests
    {
        IS3Factory? factory;
        IS3Client? client;

        public S3ClientTests(IS3Factory? factory)
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

        [SkippableTheory]
        [InlineData("items", "items/subitems/")]
        [InlineData("items/subitems/", "items")]
        [InlineData("", "items/")]
        [InlineData("items/", "")]
        [InlineData("", "")]
        public async Task FindObjectAsync_ShouldDeleteAllVersionsOfFile(string victimPrefix, string survivorPrefix)
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            int itemCounter = 3;
            // Arrange
            var fixture = new Fixture();

            var data = RandomByteArray;
            var victimName = string.Concat(victimPrefix, "c_victim_", RandomString);

            var survivorNames =
                 Enumerable.Range(0, itemCounter)
                .Select(i => string.Concat(survivorPrefix, RandomString))
                .ToArray();

            var victimVersions = Enumerable.Range(0, itemCounter)
                .Select(i => victimName)
                .ToArray();

            IEnumerable<UploadObjectResponse>? survivors = null;

            try
            {
                survivors = await UploadFilesAndAssert(survivorNames, data);
                await UploadFilesAndAssert(victimVersions, data);

                // Act
                var act = () => client!.DeleteObjectAsync(victimPrefix, victimName, CancellationToken.None);
                await act.Should().NotThrowAsync();

                // Assert
                var file = await client!.FindObjectAsync(victimName, CancellationToken.None);
                file.Should().BeNull();

                await survivorNames.ForEachAsync(
                    itemCounter,
                    async item => await AssertObjectIsFoundAndValid(item, data));

            }
            finally
            {
                // Cleanup
                if (survivors != null)
                    await survivors!.ForEachAsync(
                        itemCounter,
                        async item => await DeleteObjectAndAssert(item.Name!, item.FileId!));
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

        async Task<IEnumerable<UploadObjectResponse>> UploadFilesAndAssert(IEnumerable<string> keys, byte[] data)
        {
            var files = new ConcurrentBag<UploadObjectResponse>();

            await keys.ForEachAsync(3, async item =>
            {
                var response = await client!.UploadObjectAsync(new UploadObjectRequest
                {
                    Key = item,
                    Data = data
                }, CancellationToken.None);

                response.Should().NotBeNull();
                response!.FileId.Should().NotBeNullOrEmpty();
                files.Add(response);
            });

            return files;
        }


        static string RandomString => new Fixture().Create<string>();

        static byte[] RandomByteArray => Converter.ToByteArray(RandomString);
    }
}
