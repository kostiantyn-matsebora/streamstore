using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;

namespace StreamStore.Database
{
    public abstract class StreamDatabaseBase : IStreamDatabase
    {
        public async Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            if (expectedStreamVersion < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedStreamVersion), "Expected stream version must be equal or greater than 0.");

            var actualRevision = await GetActualRevision(streamId);
            if (actualRevision != expectedStreamVersion)
                throw new OptimisticConcurrencyException(expectedStreamVersion, actualRevision, streamId);
            
            return await BeginAppendAsyncInternal(streamId, expectedStreamVersion, token);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            await DeleteAsyncInternal(streamId, token);
        }

        public async Task<EventMetadataRecordCollection?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            return await FindMetadataAsyncInternal(streamId, token);
        }

        public async Task<EventRecordCollection> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
           if (startFrom <= 0)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Revision must be equal or greater 1.");

           return new EventRecordCollection(await ReadAsyncInternal(streamId, startFrom, count, token));
        }

        protected abstract Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default);
        protected abstract Task DeleteAsyncInternal(Id streamId, CancellationToken token = default);
        protected abstract Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
        protected abstract Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default);
        protected abstract Task<int> GetActualRevision(Id streamId, CancellationToken token = default);

    }
}
