using System.Threading.Tasks;
using System;

using StreamStore.Exceptions;
using System.Threading;



namespace StreamStore.InMemory
{
    sealed class InMemoryStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly InMemoryStreamDatabase database;

        public InMemoryStreamUnitOfWork(Id streamId, Revision expectedRevision, InMemoryStreamDatabase database, StreamRecord? existing): base(streamId, expectedRevision, existing)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        protected override Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var record = new StreamRecord(streamId, uncommited);

            database.store.AddOrUpdate(streamId, record, (key, oldValue) =>
            {
                if (oldValue.Revision != expectedRevision)
                    throw new OptimisticConcurrencyException(expectedRevision, oldValue.Revision, key);

                oldValue.AddRange(uncommited);
                return oldValue;
            });

            return Task.CompletedTask;
        }
    }
}
