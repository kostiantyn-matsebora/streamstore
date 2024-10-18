using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Storage;

namespace StreamStore.S3.Concurrency
{
    internal class S3StreamContext
    {
        public Revision ExpectedRevision { get; }
        public S3StreamContainer Transient { get; }
        public S3StreamContainer Persistent { get; }
        public Id StreamId { get; }

        public Id TransactionId { get; }

        public bool NotEmpty => Transient.NotEmpty;

        public S3StreamContext(Id streamId, Revision expectedRevision, IS3Storage storage)
        {
            TransactionId = Guid.NewGuid().ToString();
            ExpectedRevision = expectedRevision;
            Transient = storage.Transient.GetContainer(new S3ContainerPath(streamId).Combine(TransactionId));
            Persistent = storage.Persistent.GetContainer(streamId);
            StreamId = streamId;
         
        }

        public async Task Initialize(CancellationToken token)
        {
           await CopyPersistentMetadataToTransient(CancellationToken.None);
        }

        public async Task<EventMetadataRecordCollection> GetPersistentMetadataAsync(CancellationToken token)
        {
           await Persistent.MetadataObject.LoadAsync(token);

            if (Persistent.MetadataObject.State == S3ObjectState.Loaded)
            {
                return Persistent.MetadataObject.Events;
            }

            return new EventMetadataRecordCollection();
        }

        public async Task AddTransientEventAsync(EventRecord @event, CancellationToken token)
        {
            await Transient.AppendEventAsync(@event, token);
        }

        async Task CopyPersistentMetadataToTransient(CancellationToken token)
        {
            await Persistent.MetadataObject.LoadAsync(token);
            if (Persistent.MetadataObject.State == S3ObjectState.Loaded)
            {
                await Transient.MetadataObject
                        .ReplaceBy(Persistent.MetadataObject)
                        .UploadAsync(token);
            }
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            await Persistent.CopyFrom(Transient, token);
            await Transient.DeleteAsync(token);
        }

        public async Task RollBackAsync(CancellationToken token)
        {
            await Transient.DeleteAsync(token);
        }

        public void ResetState()
        {
            Transient.ResetState();
            Persistent.ResetState();
        }
    }
}
