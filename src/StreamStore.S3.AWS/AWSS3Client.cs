using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using StreamStore.S3.Client;

namespace StreamStore.S3.AWS
{
    internal class AWSS3Client : IS3Client
    {
        readonly AWSS3DatabaseSettings settings;
        readonly IAmazonS3 client;
        const int maxKeyCount = 100;

        public AWSS3Client(IAmazonS3 client, AWSS3DatabaseSettings  settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task CopyByFileIdAsync(string sourceFileId, string sourceName, string destinationName, CancellationToken token)
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = settings.BucketName,
                SourceVersionId = sourceFileId,
                SourceKey = sourceName,
                DestinationBucket = settings.BucketName,
                DestinationKey = destinationName,
            };

            await client.CopyObjectAsync(request);
        }

        public async Task DeleteObjectByFileIdAsync(string fileId, string key, CancellationToken token)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = settings.BucketName,
                Key = key,
                VersionId = fileId
            };

            await client.DeleteObjectAsync(deleteObjectRequest, token);
        }

        public async Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token)
        {
            var request = new GetObjectRequest
            {
                Key = key,
                BucketName = settings.BucketName
            };

            try
            {
                var response = await client!.GetObjectAsync(request);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK) return null;
                var stream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(stream, token);

                return new FindObjectResponse
                {
                    Data = stream.ToArray(),
                    Name = key,
                    FileId = response.VersionId
                };
            }
            catch (AmazonS3Exception ex) {
                if (ex.ErrorCode == "NoSuchKey") return null;
                throw;
            }
        }

        public async Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token)
        {
            var request = new ListVersionsRequest
            {
                Prefix = key,
                MaxKeys = 1,
                Delimiter = settings.Delimiter,
                BucketName = settings.BucketName
            };

            var response = await client!.ListVersionsAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK || !response.Versions.Any())
                return null;

            return new ObjectDescriptor
            {
                FileName = response.Versions[0].Key,
                FileId = response.Versions[0].VersionId
            };
        }

        public async Task<ListS3ObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectName, CancellationToken token)
        {
            var request = new ListVersionsRequest()
            {
                Prefix = !string.IsNullOrEmpty(sourcePrefix) ? sourcePrefix : null,
                MaxKeys = maxKeyCount,
                KeyMarker = !string.IsNullOrEmpty(startObjectName) ? startObjectName : null,
                Delimiter = !string.IsNullOrEmpty(sourcePrefix) ? settings.Delimiter : null,
                BucketName = settings.BucketName
            };
            var response = await client!.ListVersionsAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return new ListS3ObjectsResponse
            {
                Objects = response.Versions.Select(v => new ObjectDescriptor
                {
                    FileName = v.Key,
                    FileId = v.VersionId
                }).ToArray(),
                NextFileName = response.NextKeyMarker
            };
        }

        public async Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token)
        {
            var r = new PutObjectRequest
            {
                BucketName = settings.BucketName,
                Key = request.Key,
                InputStream = new MemoryStream(request.Data),
            };

            var response = await client.PutObjectAsync(r);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return new UploadObjectResponse
            {
                Name = request.Key,
                FileId = response.VersionId
            };
        }

        public ValueTask DisposeAsync()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
            return new ValueTask(Task.CompletedTask);
        }

        void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                client?.Dispose();
            }
        }

    }
}
