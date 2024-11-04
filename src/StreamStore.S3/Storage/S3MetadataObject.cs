using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.Serialization;



namespace StreamStore.S3.Storage
{

    internal class S3MetadataObject : S3Object
    {
        List<EventMetadataRecord> records = new List<EventMetadataRecord>();

        public EventMetadataRecordCollection Events => new EventMetadataRecordCollection(records);

        public S3MetadataObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override async Task DeleteAsync(CancellationToken token)
        {
            await base.DeleteAsync(token);
            records.Clear();
        }

        public override async Task LoadAsync(CancellationToken token)
        {
            var data = await LoadDataAsync(token);
            if (State == S3ObjectState.Loaded)
                records = Converter.FromByteArray<EventMetadataRecord[]>(data).ToList();
        }

        public S3MetadataObject AppendEventAsync(EventMetadataRecord record, CancellationToken token)
        {
            records.Add(record);
            return this;
        }

        public async Task<S3MetadataObject> ReplaceByAsync(S3MetadataObject metadataObject, CancellationToken token)
        {
            await CopyFromAsync(metadataObject, token);
            this.records = metadataObject.records;
            return this;
        }

        public override void ResetState()
        {
            base.ResetState();
            records.Clear();
        }

        public override async Task UploadAsync(CancellationToken token)
        {
            await UploadDataAsync(Converter.ToByteArray(records.ToArray()), token);
        }
    }
}
