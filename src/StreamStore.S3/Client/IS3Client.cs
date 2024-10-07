using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IAsyncDisposable
    {
        Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token);
        Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token);
        Task DeleteObjectAsync(string prefix, string? key, CancellationToken token);
        Task DeleteObjectByFileIdAsync(string fileId, string key, CancellationToken token);
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
}
