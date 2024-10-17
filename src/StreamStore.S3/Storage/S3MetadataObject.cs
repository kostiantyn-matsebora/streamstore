using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Operations;


namespace StreamStore.S3.Storage
{
    internal class S3MetadataObject: S3Object
    {
        readonly List<EventMetadataRecord> records = new List<EventMetadataRecord>();

        S3StreamMetadataRecord? record;
        byte[]? data;

        public override byte[] Data
        {
            get
            {
                return data ?? new byte[0];
            }
            set
            {
                data = value;
                record = Converter.FromByteArray<S3StreamMetadataRecord>(data)!;
            }
        }
        public S3StreamMetadataRecord? Metadata
        {
            get
            {
                return record;
            }
            set
            {
                record = value;
                data = Converter.ToByteArray(record!);
            }
        }

        public EventMetadataRecord[]? Events => record!.Events;

        public S3MetadataObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
            Metadata = new S3StreamMetadataRecord();
        }

        public async Task AppendAsync(EventRecord @event, CancellationToken token)
        {
            records.Add(@event);
            Metadata = new S3StreamMetadataRecord(records.ToArray());
            await UploadAsync(token);
        }

        public async Task UploadAsync(S3MetadataObject metadata, CancellationToken token)
        {
            Data = metadata.Data;
            await UploadAsync(token);
        }
    }
}
