using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamStore.Exceptions;
using System;
using System.Linq;
using StreamStore.Storage;


namespace StreamStore.InMemory
{

    public sealed class InMemoryStreamStorage : IStreamStorage
    {
        internal ConcurrentDictionary<string, StreamEventRecordCollection> storage;

        public InMemoryStreamStorage()
        {
            storage = new ConcurrentDictionary<string, StreamEventRecordCollection>();
        }

        public Task<StreamEventRecordCollection?> FindAsync(Id streamId, CancellationToken token = default)
        {
            if (!storage.TryGetValue(streamId, out var record))
                return Task.FromResult<StreamEventRecordCollection?>(null);

            return Task.FromResult<StreamEventRecordCollection?>(record);
        }

        public Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            if (storage.ContainsKey(streamId))
                storage.TryRemove(streamId, out var _);

            return Task.CompletedTask;
        }

        public Task<Revision?> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            if (!storage.TryGetValue(streamId, out var record))
                return Task.FromResult<Revision?>(null!);

            return Task.FromResult<Revision?>(new StreamEventMetadataRecordCollection(record).MaxRevision);
        }

        public Task<IStreamWriter> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            if (storage.TryGetValue(streamId, out var existing) && expectedStreamVersion != existing.MaxRevision)
            {
                // It seems like stream has been already added, fail fast
                throw new OptimisticConcurrencyException(expectedStreamVersion, existing.MaxRevision, streamId);
            }

            return Task.FromResult((IStreamWriter)new InMemoryStreamWriter(streamId, expectedStreamVersion, this.storage, existing));
        }

        public async Task<IStreamEventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Start from should be greater than zero.");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than zero.");

            var stream = await FindAsync(streamId, token);

            if (stream == null)
                throw new StreamNotFoundException(streamId);

            if (startFrom > stream.MaxRevision)
                return Array.Empty<IStreamEventRecord>();

            return
                new StreamEventRecordCollection(
                    stream
                        .Where(e => e.Revision >= startFrom)
                        .Take(count)).ToArray();
        }
    }
}
