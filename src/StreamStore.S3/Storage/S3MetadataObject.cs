using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.Serialization;
using StreamStore.Storage;



namespace StreamStore.S3.Storage
{

    internal class S3MetadataObject : S3Object
    {
        StreamEventMetadataRecordCollection records = new StreamEventMetadataRecordCollection();

        public StreamEventMetadataRecordCollection Events => records;

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
            {
                records = new StreamEventMetadataRecordCollection(Converter.FromByteArray<StreamEventMetadataRecord[]>(data)!);
            }
        }

        public S3MetadataObject AppendEventAsync(IStreamEventMetadata record, CancellationToken token)
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
