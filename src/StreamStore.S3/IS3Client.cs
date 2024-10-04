using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3
{
    public interface IS3Client: IDisposable
    {
        Task<MemoryStream?> FindBlobAsync(string bucketName, string key, CancellationToken token);
        Task<IMetadataCollection> FindBlobMetadataAsync(string bucketName, string key, CancellationToken token);
        Task UploadBlobAsync(string bucketName, string key, MemoryStream stream, IDictionary<string,string> metadata, CancellationToken token);
        Task DeleteBlobAsync(string bucketName, string key, CancellationToken token);
    }
}
