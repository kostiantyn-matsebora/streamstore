using System;

using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.Database
{
    public abstract class StreamDatabaseBase : IStreamDatabase
    {
        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            if (expectedStreamVersion < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedStreamVersion), "Expected stream version must be equal or greater than 0.");

            return BeginAppendAsyncInternal(streamId, expectedStreamVersion, token);
        }

        public Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            return DeleteAsyncInternal(streamId, token);
        }

        public Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            return FindMetadataAsyncInternal(streamId, token);
        }

        public Task<EventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
           if (startFrom <= 0)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Revision must be equal or greater 1.");

           return ReadAsyncInternal(streamId, startFrom, count, token);
        }

        protected abstract Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default);
        protected abstract Task DeleteAsyncInternal(Id streamId, CancellationToken token = default);
        protected abstract Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
        protected abstract Task<StreamMetadataRecord?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default);
    }
}
