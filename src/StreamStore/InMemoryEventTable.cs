using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;

namespace StreamStore
{

    public class InMemoryEventTable : IEventTable
    {
        readonly ConcurrentDictionary<string, EventRecordBatch> store = new ConcurrentDictionary<string, EventRecordBatch>();

        public Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!store.TryGetValue(streamId, out var stream))
                return Task.FromResult<StreamRecord?>(null);

            return Task.FromResult<StreamRecord?>(new StreamRecord(streamId, stream));
        }

        public Task InsertAsync(string streamId, IEnumerable<EventRecord> uncommited, CancellationToken cancellationToken)
        {
            var transient = new EventRecordBatch(uncommited);

            if (!store.TryGetValue(streamId, out var persistent))
            {
                if (!store.TryAdd(streamId, transient))  // It seems like stream has been already added
                {
                    persistent = store[streamId];
                    throw new OptimisticConcurrencyException(0, persistent.MaxRevision, streamId);
                }
                return Task.CompletedTask;
            }

            lock (persistent) // Prevent race condition
            {
                new Validator()
                    .Uncommited(transient)
                    .Persistent(persistent)
                    .StreamId(streamId)
                    .Validate();

                persistent.AddRange(uncommited);
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            if (store.ContainsKey(streamId))
                store.TryRemove(streamId, out var commited);

            return Task.CompletedTask;
        }

        public Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!store.TryGetValue(streamId, out var stream))
                return Task.FromResult<StreamMetadataRecord?>(null);

            return Task.FromResult<StreamMetadataRecord?>(new StreamMetadataRecord(streamId, stream.Events));
        }

        class EventRecordBatch : EventBatch<EventRecord>
        {
            public EventRecordBatch(IEnumerable<EventRecord> records) : base(records)
            {
            }
        }
    }
}
