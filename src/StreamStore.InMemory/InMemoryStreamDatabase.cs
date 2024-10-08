using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamStore.Exceptions;


namespace StreamStore.InMemory
{

    public class InMemoryStreamDatabase : IStreamDatabase
    {
        internal ConcurrentDictionary<string, StreamRecord> store = new ConcurrentDictionary<string, StreamRecord>();

        public Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<StreamRecord?>(null);

            return Task.FromResult<StreamRecord?>(record);
        }

        public Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            if (store.ContainsKey(streamId))
                store.TryRemove(streamId, out var commited);

            return Task.CompletedTask;
        }

        public Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!store.TryGetValue(streamId, out var record))
                return Task.FromResult<StreamMetadataRecord?>(null);

            return Task.FromResult<StreamMetadataRecord?>(new StreamMetadataRecord(streamId, record.Events));
        }

        public IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0)
        {
            if (store.TryGetValue(streamId, out var existing) && expectedStreamVersion != existing.Revision)
            {
                // It seems like stream has been already added, fail fast
                throw new OptimisticConcurrencyException(expectedStreamVersion, existing.Revision, streamId);
            }

            return new InMemoryStreamUnitOfWork(streamId, expectedStreamVersion, this, existing);
        }
    }
}
