using System;
using System.Threading;
using System.Threading.Tasks;
using B2Net;
using B2Net.Models;
using StreamStore.S3.Client;

namespace StreamStore.S3.B2
{
    internal class B2S3Client : IS3Client
    {
        readonly B2StreamDatabaseSettings settings;
        IB2Client? client;

        public B2S3Client(B2StreamDatabaseSettings settings, IB2Client client)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            client.Authorize();
        }

        public async Task DeleteObjectAsync(string key, CancellationToken token, string? fileId = null)
        {
            await client!.Files.Delete(fileId, key, token);
        }

        public async Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token)
        {
            try
            {
                var file = await client!.Files.DownloadByName(key, settings.BucketName, token);
                return new FindObjectResponse
                {
                    Data = file.FileData,
                    FileId = file.FileId
                };
            }
            catch (B2Exception ex)
            {
                if (ex.Code == "not_found")
                    return null;
                throw;
            }
        }

        public async Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token)
        {
            var uploadUrl = await client!.Files.GetUploadUrl(settings.BucketId);
            var file = await client!.Files.Upload(request.Data, request.Key, uploadUrl, string.Empty, null, token);
            if (file == null) return null;

            return new UploadObjectResponse
            {
                FileId = file.FileId
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