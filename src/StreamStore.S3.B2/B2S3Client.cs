using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Extensions;
using Bytewizer.Backblaze.Models;
using StreamStore.S3.Client;


namespace StreamStore.S3.B2
{
    internal class B2S3Client : IS3Client
    {
        readonly B2StreamDatabaseSettings settings;
        readonly int maxDegreeOfParallelism = 10;
        IStorageClient? client;
        const int maxFileCount = 10000;

        public B2S3Client(B2StreamDatabaseSettings settings, IStorageClient client)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task DeleteObjectAsync(string prefix, string? key, CancellationToken token)
        {
            var request = new ListFileVersionRequest(settings.BucketId)
            {
                Prefix = !string.IsNullOrEmpty(prefix) ? prefix : null,
                StartFileName = key,
                MaxFileCount = maxFileCount,
                Delimiter = !string.IsNullOrEmpty(prefix) ? settings.Delimiter : null,
            };

            List<FileItem> victims = new List<FileItem>();

            do
            {
                var files = await client!.Files.ListVersionsAsync(request);

                if (!files.IsSuccessStatusCode)
                    return;

                victims = files.Response.Files;
                if (!string.IsNullOrEmpty(key))
                    victims = victims.Where(f => f.FileName == key).ToList();

                await victims.ForEachAsync(
                    maxDegreeOfParallelism,
                    async victim =>
                    {
                      await client!.Files.DeleteAsync(victim.FileId, victim.FileName);
                    });

            } while (victims.Any());
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