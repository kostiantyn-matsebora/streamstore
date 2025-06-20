﻿using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.Storage;


namespace StreamStore.S3.Storage
{
    internal class S3StreamStorage : S3ContainerStorage<S3StreamContainer>
    {
        public S3StreamStorage(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public async Task<S3StreamContainer> LoadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            var stream = GetContainer(streamId);
            await stream.LoadAsync(startFrom, count, token);
            return stream;
        }
 
        public async Task<S3MetadataObject> LoadMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetContainer(streamId);
            await stream.MetadataObject.LoadAsync(token);
            return stream.MetadataObject;
        }

        public async Task AppendEventAsync(string streamId, StreamEventRecord record, CancellationToken token)
        {
            var stream = GetContainer(streamId);
            await stream.AppendEventAsync(record, token);
        }

        protected override S3StreamContainer CreateContainer(string name)
        {
           return new S3StreamContainer(path.Combine(name), clientFactory);
        }
    }
}
