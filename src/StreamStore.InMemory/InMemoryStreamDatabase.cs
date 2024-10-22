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
        internal ConcurrentDictionary<string, StreamRecord> store = new ConcurrentDictionary<string, StreamRecord>();

        public Task<StreamRecord?> FindAsync(Id streamId, CancellationToken token = default)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<StreamRecord?>(null);

            return Task.FromResult<StreamRecord?>(record);
        }

        public Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            if (store.ContainsKey(streamId))
                store.TryRemove(streamId, out var commited);

            return Task.CompletedTask;
        }

        public Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<StreamMetadataRecord?>(null);

            return Task.FromResult<StreamMetadataRecord?>(new StreamMetadataRecord(record.Events));
        }

        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            if (store.TryGetValue(streamId, out var existing) && expectedStreamVersion != existing.Revision)
            {
                // It seems like stream has been already added, fail fast
                throw new OptimisticConcurrencyException(expectedStreamVersion, existing.Revision, streamId);
            }

            return Task.FromResult((IStreamUnitOfWork)new InMemoryStreamUnitOfWork(streamId, expectedStreamVersion, this, existing));
        }

        public async Task<EventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Start from should be greater than zero.");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than zero.");

            var stream = await FindAsync(streamId, token);

            if (stream == null)
                throw new StreamNotFoundException(streamId);

            if (startFrom > stream.Revision)
                return Array.Empty<EventRecord>();

            return 
                stream.Events.Where(e => e.Revision >= startFrom)
                .Take(count)
                .ToArray();
        }
    }
}
