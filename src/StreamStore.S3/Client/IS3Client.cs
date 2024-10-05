using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IDisposable
    {
        Task<IEnumerable<byte[]>?> FindObjectsByPrefixAsync(string prefix, CancellationToken token);
        Task<byte[]?> FindObjectAsync(string key, CancellationToken token);
        Task UploadObjectAsync(string key, byte[] data, IS3ReadonlyMetadataCollection metadata,  CancellationToken token, bool lockObject = false);
        Task DeleteObjectAsync(string key, CancellationToken token);
    }
}
