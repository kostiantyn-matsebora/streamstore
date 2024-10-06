using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;

namespace StreamStore.S3.B2
{
    internal class B2S3Client : IS3Client
    {
        public Task DeleteObjectAsync(string key, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string?> FindObjectAsync(string key, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UploadObjectAsync(string key, string data, IS3ReadonlyMetadataCollection metadata, CancellationToken token, bool lockObject = false)
        {
            throw new NotImplementedException();
        }
    }
}
