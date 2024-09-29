using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;


namespace StreamStore.InMemory
{

    public class InMemoryDatabase : IEventDatabase
    {
        internal ConcurrentDictionary<string, StreamRecord> store = new ConcurrentDictionary<string, StreamRecord>();

        public Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!store.TryGetValue(streamId, out var stream))
                return Task.FromResult<StreamRecord?>(null);

            return Task.FromResult<StreamRecord?>(stream);
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

        public IEventUnitOfWork CreateUnitOfWork(string streamId, int expectedStreamVersion = 0)
        {
            if (store.TryGetValue(streamId, out var existing))
            {
                if (expectedStreamVersion != existing.Revision) // It seems like stream has been already added, fail fast
                    throw new OptimisticConcurrencyException(expectedStreamVersion, existing.Revision, streamId);
            }

            return new InMemoryEventUnitOfWork(streamId, expectedStreamVersion, this, existing);
        }
    }
}
