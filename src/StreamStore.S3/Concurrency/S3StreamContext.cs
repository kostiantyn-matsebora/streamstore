﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Operations;
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

        public bool HasChanges => Transient.Events.Any();

          public S3StreamContext(Id streamId, Revision expectedRevision, S3Storage storage)
        {
            TransactionId = Guid.NewGuid().ToString();
            ExpectedRevision = expectedRevision;
            Transient = storage.Transient.GetChild(new S3ContainerPath(streamId).Combine(TransactionId));
            Persistent = storage.Persistent.GetChild(streamId);
            StreamId = streamId;
         
        }

        public async Task<S3StreamMetadataRecord> GetPersistentMetadataAsync(CancellationToken token)
        {
           await Persistent.MetadataObject.LoadAsync(token);

            if (Persistent.MetadataObject.State == S3ObjectState.Loaded)
            {
                return Persistent.MetadataObject.Metadata!;
            }

            return new S3StreamMetadataRecord();
        }

        public async Task AddTransientEventAsync(EventRecord @event, CancellationToken token)
        {
           await Transient.AppendAsync(@event, token);
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
