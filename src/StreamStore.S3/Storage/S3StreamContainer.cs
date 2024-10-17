using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;



namespace StreamStore.S3.Storage
{
    internal class S3StreamContainer : S3ObjectContainer
    {
        public S3EventStorage Events { get; }

        public S3MetadataObject MetadataObject { get; }

        public EventMetadataRecord[] EventsMetadata => MetadataObject.Events!;

        public S3ObjectState State => MetadataObject.State;

        public S3StreamContainer(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {

            Events = new S3EventStorage(path.Combine("events"), clientFactory);
            MetadataObject = new S3MetadataObject(path.Combine("__metadata"), clientFactory);
        }

        public async Task LoadAsync(CancellationToken token = default)
        {
            await MetadataObject.LoadAsync(token);

            if (MetadataObject.State == S3ObjectState.NotExists) return;

            var tasks = MetadataObject.Events.Select(async e =>
            {
                await Events.LoadAsync(e.Id, token);
            });

            await Task.WhenAll(tasks);
        }

        public async Task AppendAsync(EventRecord record, CancellationToken token)
        {
            await Events.AppendAsync(record, token);
            await MetadataObject.AppendAsync(record, token);
        }

        public async Task DeleteAsync(CancellationToken token)
        {
            // Delete all events
            await Events.DeleteAsync(token);

            // Delete metadata
            await MetadataObject.DeleteAsync(token);
        }

        public async Task CopyFrom(S3StreamContainer source, CancellationToken token)
        {
            // Copy events
            foreach (var @event in source.Events)
            {
                await Events.AppendAsync(@event.Event!, token);
            }

            // Copy metadata
            await MetadataObject.UploadAsync(source.MetadataObject, token);
        }

   
        public override void ResetState()
        {
            base.ResetState();
            MetadataObject.ResetState();
            Events.ResetState();
        }
    }


}
