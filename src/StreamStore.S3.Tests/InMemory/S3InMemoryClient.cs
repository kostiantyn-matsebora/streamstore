using System.Collections.Concurrent;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemoryClient : IS3Client
    {

        static readonly ConcurrentDictionary<string, byte[]> objects = new();

        public Task DeleteObjectByVersionIdAsync(string? versionId, string key, CancellationToken token)
        {
            objects.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task<FindObjectResponse?> FindObjectAsync(string key, CancellationToken token)
        {
            if (objects.TryGetValue(key, out var data))
            {
                return Task.FromResult<FindObjectResponse?>(new FindObjectResponse
                {
                    Data = data,
                    Key = key,
                    VersionId = key
                });
            }
            return Task.FromResult<FindObjectResponse?>(null);
        }

        public Task<UploadObjectResponse?> UploadObjectAsync(UploadObjectRequest request, CancellationToken token)
        {
            objects.TryRemove(request.Key!, out _);
            objects.TryAdd(request.Key!, request.Data!);
            
            return Task.FromResult<UploadObjectResponse?>(new UploadObjectResponse
            {
                VersionId = request.Key,
                Key = request.Key
            });
        }

        public Task<ListS3ObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectKey, CancellationToken token)
        {
            if (startObjectKey == null)
            {
                var response = new ListS3ObjectsResponse
                {
                    Objects = objects.Keys
                            .Where(k => k.StartsWith(sourcePrefix))
                            .Select(k => new ObjectDescriptor
                            {
                                Key = k,
                                VersionId = k
                            }).ToArray()
                };

                return Task.FromResult<ListS3ObjectsResponse?>(response);
            }

            return Task.FromResult<ListS3ObjectsResponse?>(new ListS3ObjectsResponse
            {
                Objects = Enumerable.Empty<ObjectDescriptor>().ToArray(),
                NextObjectKey = Guid.NewGuid().ToString()
            });
        }

        public Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token)
        {
            if (objects.TryGetValue(key, out var _))
            {
                return Task.FromResult<ObjectDescriptor?>(new ObjectDescriptor
                {
                    Key = key,
                    VersionId = key
                });
            }
            return Task.FromResult<ObjectDescriptor?>(null);
        }

        public Task CopyByVersionIdAsync(string sourceVersionId, string sourceKey, string destinationKey, CancellationToken token)
        {
            lock (objects)
            {
                if (objects.TryGetValue(sourceVersionId, out var data))
                {
                    if (objects.ContainsKey(destinationKey))
                    {
                        objects.TryRemove(destinationKey, out _);
                    }
                    objects.TryAdd(destinationKey, data);
                }
            }
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }
    }
}
