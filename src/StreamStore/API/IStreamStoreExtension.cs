using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StreamStore
{
    public static class IStreamStoreExtension
    {
        public static async Task<IStreamUnitOfWork> BeginWriteAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginAppendAsync(streamId, Revision.Zero, cancellationToken);
        }

        public static async Task<IAsyncEnumerable<IStreamEvent>> BeginReadAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, Revision.One, cancellationToken);
        }

        public static async Task<IStreamEvent[]> ReadToEndAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, cancellationToken).ReadToEndAsync(cancellationToken);
        }

        public static async Task<IStreamEvent[]> ReadToEndAsync(this IStreamStore store, Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, expectedRevision, cancellationToken).ReadToEndAsync(cancellationToken);
        }

        public static Task WriteAsync(this IStreamStore store, Id streamId, Revision expectedRevision, IEnumerable<IEventEnvelope> events, CancellationToken token = default)
        {
            return store.BeginAppendAsync(streamId, expectedRevision, token).WriteAsync(events, token);
        }

        public static Task WriteAsync(this IStreamStore store, Id streamId, IEnumerable<IEventEnvelope> events, CancellationToken token = default)
        {
            return store.BeginWriteAsync(streamId,  token).WriteAsync(events, token);
        }
    }
}
