using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamStore.Exceptions;
using System;
using System.Linq;


namespace StreamStore.InMemory
{

    public sealed class InMemoryStreamDatabase : IStreamDatabase
    {
        internal ConcurrentDictionary<string, EventRecordCollection> store;

        public InMemoryStreamDatabase()
        {
            store = new ConcurrentDictionary<string, EventRecordCollection>();
        }

        public Task<EventRecordCollection?> FindAsync(Id streamId, CancellationToken token = default)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<EventRecordCollection?>(null);

            return Task.FromResult<EventRecordCollection?>(record);
        }

        public Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            if (store.ContainsKey(streamId))
                store.TryRemove(streamId, out var _);

            return Task.CompletedTask;
        }

        public Task<EventMetadataRecordCollection?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<EventMetadataRecordCollection?>(null);

            return Task.FromResult<EventMetadataRecordCollection?>(new EventMetadataRecordCollection(record));
        }

        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            if (store.TryGetValue(streamId, out var existing) && expectedStreamVersion != existing.MaxRevision)
            {
                // It seems like stream has been already added, fail fast
                throw new OptimisticConcurrencyException(expectedStreamVersion, existing.MaxRevision, streamId);
            }

            return Task.FromResult((IStreamUnitOfWork)new InMemoryStreamUnitOfWork(streamId, expectedStreamVersion, this, existing));
        }

        public async Task<EventRecordCollection> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Start from should be greater than zero.");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than zero.");

            var stream = await FindAsync(streamId, token);

            if (stream == null)
                throw new StreamNotFoundException(streamId);

            if (startFrom > stream.MaxRevision)
                return new EventRecordCollection();

            return
                new EventRecordCollection(
                    stream
                        .Where(e => e.Revision >= startFrom)
                        .Take(count)
                        .ToArray());
        }
    }
}
