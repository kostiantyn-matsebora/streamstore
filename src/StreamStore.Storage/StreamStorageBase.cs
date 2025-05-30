using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.Exceptions.Reading;
using StreamStore.Extensions;

namespace StreamStore.Storage
{
    public abstract class StreamStorageBase<TEntity> : IStreamStorage
    {
        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            await DeleteAsyncInternal(streamId, token);
        }

        public async Task<IStreamMetadata?> GetMetadataAsync(Id streamId, CancellationToken token = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            return await GetMetadataInternal(streamId);
        }

        public async Task WriteAsync(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            await WriteAsyncInternal(streamId, batch, token);
        }

        public async Task<IStreamEventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            if (startFrom <= Revision.Zero)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Revision is less than or equal to zero");

            var metadata = await GetMetadataInternal(streamId);


            if (metadata == null || metadata.Revision == Revision.Zero)
                throw new StreamNotFoundException(streamId);

            var entities = await ReadAsyncInternal(streamId, startFrom, count, token);

            if (entities.NullOrEmpty())
                return Array.Empty<IStreamEventRecord>();

            var result = new List<IStreamEventRecord>();

            foreach ( var entity in entities)
            {
                var builder = new StreamEventRecordBuilder();
                BuildRecord(builder, entity);
                result.Add(builder.Build());
            }
           
            return result.ToArray();
        }

        protected abstract void BuildRecord(IStreamEventRecordBuilder builder, TEntity entity);
        protected abstract Task<TEntity[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default);
        protected abstract Task DeleteAsyncInternal(Id streamId, CancellationToken token = default);
        protected abstract Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default);
        protected abstract Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default);
    }
}
