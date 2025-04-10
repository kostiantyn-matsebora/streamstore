using System.Threading.Tasks;
using System;

using StreamStore.Exceptions;
using System.Threading;
using StreamStore.Storage;
using System.Collections.Concurrent;



namespace StreamStore.InMemory
{
    sealed class InMemoryStreamWriter : StreamWriterBase
    {
        readonly ConcurrentDictionary<string, StreamEventRecordCollection> storage;

        public InMemoryStreamWriter(Id streamId, Revision expectedRevision, ConcurrentDictionary<string, StreamEventRecordCollection> storage, StreamEventRecordCollection? existing): base(streamId, expectedRevision, existing)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        protected override Task CommitAsync(StreamEventRecordCollection uncommited, CancellationToken token)
        {
            var record = new StreamEventRecordCollection(uncommited);

            storage.AddOrUpdate(streamId, record, (key, oldValue) =>
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
