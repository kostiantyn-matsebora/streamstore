using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using StreamStore.Models;

namespace StreamStore
{
    public static class StreamStoreExtension
    {
        public static async Task<IStreamUnitOfWork> BeginWriteAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginWriteAsync(streamId, Revision.Zero, cancellationToken);
        }

        public static async Task<IAsyncEnumerable<StreamEvent>> BeginReadAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, Revision.One, cancellationToken);
        }

        public static async Task<StreamEventCollection> ReadToEndAsync(this IStreamStore store, Id streamId, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, cancellationToken).ReadToEndAsync(cancellationToken);
        }

        public static async Task<StreamEventCollection> ReadToEndAsync(this IStreamStore store, Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            return await store.BeginReadAsync(streamId, expectedRevision, cancellationToken).ReadToEndAsync(cancellationToken);
        }

        public static Task<Revision> WriteAsync(this IStreamStore store, Id streamId, Revision expectedRevision, IEnumerable<Event> events, CancellationToken token = default)
        {
            return store.BeginWriteAsync(streamId, expectedRevision, token).WriteAsync(events, token);
        }

        public static Task<Revision> WriteAsync(this IStreamStore store, Id streamId, IEnumerable<Event> events, CancellationToken token = default)
        {
            return store.BeginWriteAsync(streamId,  token).WriteAsync(events, token);
        }
    }
}
