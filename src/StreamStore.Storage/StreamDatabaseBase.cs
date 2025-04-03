using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;

namespace StreamStore.Database
{
    public abstract class StreamDatabaseBase : IStreamDatabase
    {
        public async Task<IStreamWriter> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            if (expectedStreamVersion < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedStreamVersion), "Expected stream version must be equal or greater than 0.");

            var actualRevision = await GetActualRevisionInternal(streamId);
            if (actualRevision == null) actualRevision = Revision.Zero;
            if (actualRevision != expectedStreamVersion)
                throw new OptimisticConcurrencyException(expectedStreamVersion, actualRevision.Value, streamId);
            
            return await BeginAppendAsyncInternal(streamId, expectedStreamVersion, token);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            await DeleteAsyncInternal(streamId, token);
        }

        public async Task<Revision?> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            var actualRevision = await GetActualRevisionInternal(streamId);
            return actualRevision == Revision.Zero ? null : (Revision?)actualRevision;
        }

        public async Task<EventRecordCollection> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
           if (startFrom <= 0)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Revision must be equal or greater 1.");

           var actualRevision = await GetActualRevisionInternal(streamId);

            if (actualRevision == null || actualRevision == Revision.Zero)
                throw new StreamNotFoundException(streamId);

            return new EventRecordCollection(await ReadAsyncInternal(streamId, startFrom, count, token));
        }

        protected abstract Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default);
        protected abstract Task DeleteAsyncInternal(Id streamId, CancellationToken token = default);
        protected abstract Task<IStreamWriter> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
        protected abstract Task<Revision?> GetActualRevisionInternal(Id streamId, CancellationToken token = default);

    }
}
