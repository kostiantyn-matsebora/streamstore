using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3StreamStorage : S3ObjectStorage<S3StreamContainer>
    {
        public S3StreamStorage(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public async Task<S3StreamContainer> LoadAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetContainer(streamId);
            await stream.LoadAsync(token);
            return stream;
        }
 
        public async Task<S3MetadataObject> LoadMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetContainer(streamId);
            await stream.MetadataObject.LoadAsync(token);
            return stream.MetadataObject;
        }

        public async Task AppendEventAsync(string streamId, EventRecord record, CancellationToken token)
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
