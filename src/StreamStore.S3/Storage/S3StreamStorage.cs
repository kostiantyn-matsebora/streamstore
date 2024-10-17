using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3StreamStorage : S3ObjectContainer<S3StreamContainer>
    {
        public S3StreamStorage(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override S3StreamContainer CreateChild(string name)
        {
            return new S3StreamContainer(path.Combine(name), clientFactory);
        }

        public async Task<S3StreamContainer> LoadAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetChild(streamId);
            await stream.LoadAsync(token);
            return stream;
        }
        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetChild(streamId);
            await stream.DeleteAsync(token);
        }

        public async Task<S3MetadataObject> LoadMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var stream = GetChild(streamId);
            await stream.MetadataObject.LoadAsync(token);
            return stream.MetadataObject;
        }

        public async Task AppendAsync(string streamId, EventRecord record, CancellationToken token)
        {
            var stream = GetChild(streamId);
            await stream.AppendAsync(record, token);
        }
    }
}
