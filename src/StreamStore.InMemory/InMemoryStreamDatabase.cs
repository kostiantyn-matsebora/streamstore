using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamStore.Exceptions;


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

        public Task<EventRecord[]> ReadAsync(Id streamId, Revision start, int count, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
