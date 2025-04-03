using System.Threading.Tasks;
using System;

using StreamStore.Exceptions;
using System.Threading;
using StreamStore.Storage;



namespace StreamStore.InMemory
{
    sealed class InMemoryStreamUnitOfWork : StreamWriterBase
    {
        readonly InMemoryStreamStorage storage;

        public InMemoryStreamUnitOfWork(Id streamId, Revision expectedRevision, InMemoryStreamStorage storage, EventRecordCollection? existing): base(streamId, expectedRevision, existing)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        protected override Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var record = new EventRecordCollection(uncommited);

            storage.store.AddOrUpdate(streamId, record, (key, oldValue) =>
            {
                if (oldValue.MaxRevision != expectedRevision)
                    throw new OptimisticConcurrencyException(expectedRevision, oldValue.MaxRevision, key);

                oldValue.AddRange(uncommited);
                return oldValue;
            });

            return Task.CompletedTask;
        }
    }
}
