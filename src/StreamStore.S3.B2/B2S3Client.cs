using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using StreamStore.S3.Client;



namespace StreamStore.S3.B2
{
    internal class B2S3Client : IS3Client
    {
        readonly B2StreamDatabaseSettings settings;
        IStorageClient? client;
        const int maxFileCount = 100;

        public B2S3Client(B2StreamDatabaseSettings settings, IStorageClient client)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token)
        {
            var request = new ListFileVersionRequest(settings.BucketId)
            {
                Prefix = key,
                MaxFileCount = 1,
                Delimiter = settings.Delimiter
            };

            var files = await client!.Files.ListVersionsAsync(request);

            if (!files.IsSuccessStatusCode || !files.Response.Files.Any())
                return null;

            return new ObjectDescriptor
            {
                FileName = files.Response.Files.First().FileName,
                FileId = files.Response.Files.First().FileId
            };
        }

        public async Task<ListObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectName, CancellationToken token)
        {
            var request = new ListFileVersionRequest(settings.BucketId)
            {
                Prefix = !string.IsNullOrEmpty(sourcePrefix) ? sourcePrefix : null,
                MaxFileCount = maxFileCount,
                StartFileId = !string.IsNullOrEmpty(startObjectName) ? startObjectName : null,
                Delimiter = !string.IsNullOrEmpty(sourcePrefix) ? settings.Delimiter : null,
            };
            var files = await client!.Files.ListVersionsAsync(request);

            if (!files.IsSuccessStatusCode)
                return null;

            return new ListObjectsResponse
            {
                Objects = files.Response.Files.Select(f => new ObjectDescriptor
                {
                    FileName = f.FileName,
                    FileId = f.FileId
                }).ToArray(),
                NextFileName = files.Response.NextFileName
            };
        }


        public async Task CopyByFileIdAsync(string sourceFileId, string destinationName, CancellationToken token)
        {
            var copyRequest = new CopyFileRequest(
                        sourceFileId,
                        destinationName);

            await client!.Files.CopyAsync(copyRequest);
        }

        public async Task DeleteObjectByFileIdAsync(string fileId, string key, CancellationToken token)
        {
            await client!.Files.DeleteAsync(fileId, key);
            return;

        }

        public async Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token)
        {
            using var stream = new MemoryStream();

            var file = await client!.DownloadAsync(settings.BucketName, key, stream);

            if (!file.IsSuccessStatusCode) return null;
            return new FindObjectResponse
            {
                Data = stream.ToArray(),
                Name = file.Response.FileName,
                FileId = file.Response.FileId
            };
        }

        public async Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token)
        {
            using var stream = new MemoryStream(request.Data);

            var response = await client!.UploadAsync(settings.BucketId, request.Key, stream);

            if (!response.IsSuccessStatusCode) return null;

            return new UploadObjectResponse
            {
                FileId = response.Response.FileId,
                Name = response.Response.FileName
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
                client = null;
            }
        }


    }


}