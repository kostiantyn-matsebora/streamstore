using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamDB.Contracts;


namespace StreamDB
{
    public sealed class StreamDB: IStreamDB
    {
        readonly IEventStore store;

        public StreamDB(IEventStore store)
        {
            this.store = store;
        }

        public async Task SaveAsync(string streamId, EventEnvelope[] uncommited, int expectedRevision, CancellationToken ct = default)
        {
            if (streamId == null) throw new ArgumentNullException(nameof(streamId));

            var data = await store.FindAsync(streamId, ct);
            
            ThrowIfVersionConflict(expectedRevision, data.Revision, streamId);
            ThrowIfDuplicatesAreFound(uncommited, data.Events, streamId);
            
            var revision = data.Revision;

            await store.InsertAsync(streamId, uncommited.Select(e => e.ToEventData(revision++)).ToArray(), ct);
        }

        private void ThrowIfDuplicatesAreFound(EventEnvelope[] uncommited, EventData[] commited, Id streamId)
        {
            var ids = uncommited
                    .Select(e => (string)e.Id)
                    .Concat(commited.Select(e => e.Id))
                    .ToArray();

            var duplicates = 
                ids.GroupBy(id => id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToArray();

            if (duplicates.Any())
                throw new DuplicateEventException(duplicates, streamId);
        }

        private void ThrowIfVersionConflict(int expectedRevision, int actualRevision, Id streamId)
        {
            if (expectedRevision != actualRevision)
                throw new OptimisticConcurrencyException(expectedRevision, actualRevision, streamId);
        }

        public async Task DeleteAsync(string streamId, CancellationToken ct = default)
        {
            if (streamId == null) throw new ArgumentNullException(nameof(streamId));
            
            await store.DeleteAsync(streamId, ct);
        }

        public async Task<Stream> GetAsync(string streamId, CancellationToken ct = default)
        {
            var data = await store.FindAsync(streamId, ct);
            if (data == null) 
                throw new StreamNotFoundException(streamId);

            return new Stream(streamId, data.Revision, data.Events.Select(e => e.ToEvent()).ToArray());
        }
    }
}
