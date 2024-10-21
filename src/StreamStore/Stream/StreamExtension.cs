using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;

namespace StreamStore
{
    public static class StreamExtension
    {
        public static async Task<IEventStreamWriter> BeginWriteAsync(this Task<IStream> stream, CancellationToken cancellationToken = default)
        {
            return await stream.Result.BeginWriteAsync(Revision.Zero, cancellationToken);
        }

        public static async Task<IEventStreamWriter> BeginWriteAsync(this IStream stream, CancellationToken cancellationToken = default)
        {
            return await stream.BeginWriteAsync(Revision.Zero, cancellationToken);
        }

        public static async Task<IEventStreamWriter> BeginWriteAsync(this Task<IStream> stream, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            return await stream.Result.BeginWriteAsync(expectedRevision, cancellationToken);
        }

        public static async Task<IEventStreamWriter> BeginWriteAsync(this IStream stream, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            return await stream.BeginWriteAsync(expectedRevision, cancellationToken);
        }

        public static IEventStreamReader BeginRead(this IStream stream, CancellationToken cancellationToken = default)
        {
            return stream.BeginRead(Revision.Zero, cancellationToken);
        }


        public static IEventStreamReader BeginRead(this Task<IStream> stream, CancellationToken cancellationToken = default)
        {
            return stream.BeginRead(Revision.Zero, cancellationToken);
        }

        public static IEventStreamReader BeginRead(this Task<IStream> stream, Revision startFrom, CancellationToken cancellationToken = default)
        {
            return stream.Result.BeginRead(startFrom, cancellationToken);
        }

        public async static Task<Revision> WriteAsync(this Task<IStream> stream, IEnumerable<Event> events, CancellationToken token)
        {
            return await stream.BeginWriteAsync(token).WriteAsync(events, token);
        }

        public async static Task<Revision> WriteAsync(this Task<IStream> stream, Id eventId, DateTime timestamp, object @event, CancellationToken token)
        {
            return await stream.BeginWriteAsync(token).WriteAsync(eventId, timestamp, @event, token);
        }

        public async static Task<Revision> WriteAsync(this Task<IStream> stream, Revision expectedRevision, IEnumerable<Event> events, CancellationToken token)
        {
            return await stream.BeginWriteAsync(expectedRevision, token).WriteAsync(events, token);
        }

        public async static Task<Revision> WriteAsync(this Task<IStream> stream, Revision expectedRevision, Id eventId, DateTime timestamp, object @event, CancellationToken token)
        {
            return await stream.BeginWriteAsync(expectedRevision, token).WriteAsync(eventId, timestamp, @event, token);
        }

        public static async Task<EventEntityCollection> ReadToEnd(this Task<IStream> stream, Revision startFrom, CancellationToken cancellationToken = default)
        {
            return await stream.Result.BeginRead(startFrom, cancellationToken).ReadToEndAsync(cancellationToken);
        }
        public static async Task<EventEntityCollection> ReadToEnd(this Task<IStream> stream, CancellationToken cancellationToken = default)
        {
            return await stream.Result.BeginRead(Revision.One, cancellationToken).ReadToEndAsync(cancellationToken);
        }
    }
}
