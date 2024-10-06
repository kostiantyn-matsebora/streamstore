using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IAsyncDisposable
    {
        Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token);
        Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token);
        Task DeleteObjectAsync(string key, CancellationToken token, string? fieldId = null);
    }


    public class FindObjectResponse
    {
        public byte[]? Data { get; set; }
        public string? FileId { get; set; }
    }

    public class UploadObjectRequest {
        public string? Key { get; set; }
        public byte[]? Data { get; set; }
    }

    public class UploadObjectResponse
    {
        public string? FileId { get; set; }
    }
}
