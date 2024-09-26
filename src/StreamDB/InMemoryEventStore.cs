using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamDB.Operations;

namespace StreamDB
{

    public class InMemoryEventStore : IEventStore
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
            var uncommitedBatch = new EventRecordBatch(uncommited);

            if (!store.TryGetValue(streamId, out var commitedBatch))
            {
                if (!store.TryAdd(streamId, uncommitedBatch))  // It seems like stream has been already added
                {
                    commitedBatch = store[streamId];
                    throw new OptimisticConcurrencyException(0, commitedBatch.MaxRevision, streamId);
                }
                return Task.CompletedTask;
            }

            lock (commitedBatch) // Prevent race condition
            {
                AppentToStreamInvariants.CheckAll(streamId, uncommitedBatch, commitedBatch);
                commitedBatch.AddRange(uncommited);
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            if (store.ContainsKey(streamId))
                store.TryRemove(streamId, out var commited);

            return Task.CompletedTask;
        }

        class EventRecordBatch : EventBatch<EventRecord>
        {
            public EventRecordBatch(IEnumerable<EventRecord> records) : base(records)
            {
            }
        }
    }
}
