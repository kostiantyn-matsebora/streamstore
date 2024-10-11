using System.Collections.Concurrent;
using System.Text;
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

        public Task<ListS3ObjectsResponse?> ListObjectsAsync(string sourcePrefix, string? startObjectName, CancellationToken token)
        {
            if (startObjectName == null)
            {
                var response = new ListS3ObjectsResponse
                {
                    Objects = objects.Keys
                            .Where(k => k.StartsWith(sourcePrefix))
                            .Select(k => new ObjectDescriptor
                            {
                                FileName = k,
                                FileId = k
                            }).ToArray()
                };

                return Task.FromResult<ListS3ObjectsResponse?>(response);
            }

            return Task.FromResult<ListS3ObjectsResponse?>(new ListS3ObjectsResponse
            {
                Objects = Enumerable.Empty<ObjectDescriptor>().ToArray(),
                NextFileName = Guid.NewGuid().ToString()
            });
        }

        public Task<ObjectDescriptor?> FindObjectDescriptorAsync(string key, CancellationToken token)
        {
            if (objects.TryGetValue(key, out var _))
            {
                return Task.FromResult<ObjectDescriptor?>(new ObjectDescriptor
                {
                    FileName = key,
                    FileId = key
                });
            }
            return Task.FromResult<ObjectDescriptor?>(null);
        }

        public Task CopyByFileIdAsync(string sourceFileId, string sourceName, string destinationName, CancellationToken token)
        {
            lock (objects)
            {
                if (objects.TryGetValue(sourceFileId, out var data))
                {
                    if (objects.ContainsKey(destinationName))
                    {
                        objects.TryRemove(destinationName, out _);
                    }
                    objects.TryAdd(destinationName, data);
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
