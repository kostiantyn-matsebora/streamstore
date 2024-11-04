using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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

        public S3EventObject SetRecord(EventRecord record)
        {
            this.record = record;

            return this;
        }

        public async Task<S3EventObject> ReplaceByAsync(S3EventObject eventObject, CancellationToken token)
        {
            await CopyFromAsync(eventObject, token);
            this.record = eventObject.record;
            return this;
        }


        public override void ResetState()
        {
            base.ResetState();
            record = null;
        }

        public override async Task UploadAsync(CancellationToken token)
        {
            if (record != null) await UploadDataAsync(Converter.ToByteArray(record!), token);
        }
    }
}
