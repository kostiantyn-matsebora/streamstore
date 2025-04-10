using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Storage;
using StreamStore.Storage;

namespace StreamStore.S3.Concurrency
{
    internal class S3StreamContext
    {
        public Revision ExpectedRevision { get; }
        internal S3StreamContainer Transient { get; }
        internal S3StreamContainer Persistent { get; }
        public Id StreamId { get; }

        public Id TransactionId { get; }

        public bool NotEmpty => Transient.NotEmpty;

        public S3StreamContext(Id streamId, Revision expectedRevision, IS3TransactionalStorage storage)
        {
            TransactionId = Guid.NewGuid().ToString();
            ExpectedRevision = expectedRevision;
            Transient = storage.GetTransientContainer(new S3ContainerPath(streamId).Combine(TransactionId));
            Persistent = storage.GetPersistentContainer(streamId);
            StreamId = streamId;
         
        }

        public async Task Initialize(CancellationToken token)
        {
           await CopyPersistentMetadataToTransient(CancellationToken.None);
        }

        public async Task<StreamEventMetadataRecordCollection> GetPersistentMetadataAsync(CancellationToken token)
        {
           await Persistent.MetadataObject.LoadAsync(token);

            if (Persistent.MetadataObject.State == S3ObjectState.Loaded)
            {
                return Persistent.MetadataObject.Events;
            }

            return new StreamEventMetadataRecordCollection();
        }

        public async Task AddTransientEventAsync(IStreamEventRecord @event, CancellationToken token)
        {
            await Transient.AppendEventAsync(@event, token);
        }

        async Task CopyPersistentMetadataToTransient(CancellationToken token)
        {
            await Persistent.MetadataObject.LoadAsync(token);
            if (Persistent.MetadataObject.State == S3ObjectState.Loaded)
            {
                await Transient.MetadataObject.ReplaceByAsync(Persistent.MetadataObject, token);
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
