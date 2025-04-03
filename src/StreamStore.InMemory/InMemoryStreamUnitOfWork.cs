using System.Threading.Tasks;
using System;

using StreamStore.Exceptions;
using System.Threading;
using StreamStore.Storage;



namespace StreamStore.InMemory
{
    sealed class InMemoryStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly InMemoryStreamDatabase database;

        public InMemoryStreamUnitOfWork(Id streamId, Revision expectedRevision, InMemoryStreamDatabase database, EventRecordCollection? existing): base(streamId, expectedRevision, existing)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        protected override Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var record = new EventRecordCollection(uncommited);

            database.store.AddOrUpdate(streamId, record, (key, oldValue) =>
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
