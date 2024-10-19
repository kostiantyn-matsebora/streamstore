using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.Serialization;


namespace StreamStore.S3.Storage
{
    class S3EventObject : S3Object
    {
        EventRecord? record;

        public EventRecord? Event => record;

        public S3EventObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override async Task LoadAsync(CancellationToken token)
        {
            var data = await LoadDataAsync(token);
            if (State == S3ObjectState.Loaded) record = Converter.FromByteArray<EventRecord>(data)!;
        }

        public override async Task DeleteAsync(CancellationToken token)
        {
            await base.DeleteAsync(token);
            record = null;
        }

        public S3EventObject ReplaceBy(EventRecord record)
        {
            this.record = record;
            return this;
        }

        public override void ResetState()
        {
            base.ResetState();
            record = null;
        }

        public override async Task UploadAsync(CancellationToken token)
        {
            if (record != null)
                await UploadDataAsync(Converter.ToByteArray(record!), token);
        }
    }
}
