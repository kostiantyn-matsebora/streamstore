using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IAsyncDisposable
    {
        Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token);
        Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token);
        Task DeleteObjectByFileIdAsync(string fileId, string key, CancellationToken token);
        Task<ListS3ObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectName, CancellationToken token);
        Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token);
        Task CopyByFileIdAsync(string sourceFileId, string sourceName, string destinationName, CancellationToken token);
    }


    public class FindObjectResponse
    {
        public byte[]? Data { get; set; }
        public string? Name { get; set; }
        public string? FileId { get; set; }
    }


    public class UploadObjectRequest {
        public string? Key { get; set; }
        public byte[]? Data { get; set; }
    }

    public class UploadObjectResponse
    {
        public string? Name { get; set; }
        public string? FileId { get; set; }
    }

    public class ListS3ObjectsResponse
    {
        public ObjectDescriptor[]? Objects { get; set; }
        public string? NextFileName { get; set; }
    }

    public class ObjectDescriptor
    {
        public string? FileName { get; set; }
        public string? FileId { get; set; }
    }
}

