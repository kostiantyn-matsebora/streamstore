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
        }

        public override  async Task LoadAsync(CancellationToken token)
        {
           await base.LoadAsync(token);
           if (State == S3ObjectState.Loaded) SynchronizeRecords();
        }

        public S3MetadataObject AppendEventAsync(EventMetadataRecord record, CancellationToken token)
        {
            records.Add(record);
            SynchronizeData();
            return this;
        }

        public S3MetadataObject ReplaceBy(S3MetadataObject metadata)
        {
            Data = metadata.Data;
            SynchronizeRecords();
            return this;
        }

        public override void ResetState()
        {
            base.ResetState();
            records.Clear();
        }

        void SynchronizeRecords()
        {
            records = Converter.FromByteArray<EventMetadataRecord[]>(Data).ToList();
        }

        void SynchronizeData()
        {
            Data = Converter.ToByteArray(records);
        }
    }
}
