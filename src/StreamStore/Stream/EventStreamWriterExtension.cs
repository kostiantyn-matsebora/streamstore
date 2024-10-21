using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public static class EventStreamWriterExtension
    {

        public static Task<Revision> WriteAsync(this Task<IEventStreamWriter> stream,IEnumerable<Event> events, CancellationToken token)
        {
            return stream.AddRangeAsync(events, token).SaveChangesAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IEventStreamWriter> stream, Id eventId, DateTime timestamp, object @event, CancellationToken token)
        {
            return stream.AddAsync(eventId, timestamp, @event).SaveChangesAsync(token);
        }

        public static Task<Revision> SaveChangesAsync(this Task<IEventStreamWriter> stream, CancellationToken token)
        {
            return stream.Result!.CommitAsync(token);
        }

        public static async Task<IEventStreamWriter> AddAsync(this IEventStreamWriter stream, Event @event, CancellationToken token = default)
        {
            await stream.AppendAsync(@event.Id, @event.Timestamp, @event.EventObject, token);
            return stream;
        }

        public static async Task<IEventStreamWriter> AddAsync(this Task<IEventStreamWriter> stream, Event @event, CancellationToken token = default)
        {
            await stream.ContinueWith(async t =>
            {
                await t.Result.AddAsync(@event);
            }, token);

            return stream.Result;
        }

        public static async Task<IEventStreamWriter> AddAsync(this Task<IEventStreamWriter> stream, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {

            await stream.ContinueWith(async t =>
            {
                await t.AddAsync(eventId, timestamp, @event, token);
            });

            return stream.Result;
        }

        public static async Task<IEventStreamWriter> AddRangeAsync(this Task<IEventStreamWriter> stream, IEnumerable<Event> events, CancellationToken token = default)
        {
            var result = await stream.ContinueWith(async t =>
             {
                 foreach (var @event in events)
                 {
                     await t.Result.AddAsync(@event);
                 }
                 return t.Result;
             }, token);

            return result.Result;
        }
    }
}
