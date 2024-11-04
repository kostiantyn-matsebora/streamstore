using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IAsyncDisposable
    {
        Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token);
        Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token);
        Task DeleteObjectByVersionIdAsync(string versionId, string key, CancellationToken token);
        Task<ListS3ObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectKey, CancellationToken token);
        Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token);
        Task CopyByVersionIdAsync(string sourceVersionId, string sourceKey, string destinationKey, CancellationToken token);
    }


    public class FindObjectResponse
    {
        public byte[]? Data { get; set; }
        public string? Key { get; set; }
        public string? VersionId { get; set; }
    }


    public class UploadObjectRequest {
        public string? Key { get; set; }
        public byte[]? Data { get; set; }
    }

    public class UploadObjectResponse
    {
        public string? Key { get; set; }
        public string? VersionId { get; set; }
    }

    public class ListS3ObjectsResponse
    {
        public ObjectDescriptor[]? Objects { get; set; }
        public string? NextObjectKey { get; set; }
    }

    public class ObjectDescriptor
    {
        public string? Key { get; set; }
        public string? VersionId { get; set; }
    }
}

