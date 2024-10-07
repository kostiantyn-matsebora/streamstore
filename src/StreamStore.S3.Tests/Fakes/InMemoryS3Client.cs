﻿using System.Collections.Concurrent;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests.Fakes
{
    internal class InMemoryS3Client : IS3Client
    {

        ConcurrentDictionary<string, byte[]> objects = new ConcurrentDictionary<string, byte[]>();


        public Task DeleteObjectByFileIdAsync(string? fileId, string key, CancellationToken token)
        {
            objects.Remove(key, out _);
            return Task.CompletedTask;
        }

        public Task DeleteObjectAsync(string prefix, string key, CancellationToken token)
        {
            throw new NotImplementedException();
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
    }
}
