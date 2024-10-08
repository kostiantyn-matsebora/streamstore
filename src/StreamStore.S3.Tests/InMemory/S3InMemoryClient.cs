using System.Collections.Concurrent;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemoryClient : IS3Client
    {

        static readonly ConcurrentDictionary<string, byte[]> objects = new();

        public Task DeleteObjectByFileIdAsync(string? fileId, string key, CancellationToken token)
        {
            objects.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task DeleteObjectAsync(string prefix, string? key, CancellationToken token)
        {
            objects.Keys.Where(k => k.StartsWith($"{prefix}{key}")).ToList().ForEach(k => objects.TryRemove(k, out _));
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token)
        {
            if (objects.TryGetValue(key, out var data))
            {
                return Task.FromResult<FindObjectResponse?>(new FindObjectResponse
                {
                    Data = data,
                    Name = key,
                    FileId = key
                });
            }
            return Task.FromResult<FindObjectResponse?>(null);
        }

        public Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token)
        {
            objects.TryAdd(request.Key!, request.Data!);
            return Task.FromResult<UploadObjectResponse?>(new UploadObjectResponse
            {
                FileId = request.Key,
                Name = request.Key
            });
        }

        public Task CopyAsync(string sourcePrefix, string destinationPrefix, CancellationToken token)
        {
            objects.Keys
                .Where(k => k.StartsWith(sourcePrefix)).ToList()
                .ForEach(k => {
                    var destinationKey = k.Replace(sourcePrefix, destinationPrefix);
                    if (objects.ContainsKey(destinationKey))
                        objects.TryRemove(destinationKey, out _);
                    objects.TryAdd(destinationKey, objects[k]);
                 });

            return Task.CompletedTask;
        }
    }
}
