using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public static class EventStreamWriterExtension
    {

        public static Task<Revision> WriteAsync(this Task<IEventStreamWriter> writer, IEnumerable<Event> events, CancellationToken token = default)
        {
            return writer.AddRangeAsync(events, token).CommitAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IEventStreamWriter> writer, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {
            return writer.AppendAsync(eventId, timestamp, @event).CommitAsync(token);
        }

        public static Task<Revision> CommitAsync(this Task<IEventStreamWriter> writer, CancellationToken token = default)
        {
            return writer.Result!.CommitAsync(token);
        }

        public static async Task<IEventStreamWriter> AppendAsync(this IEventStreamWriter writer, Event @event, CancellationToken token = default)
        {
            await writer.AppendAsync(@event.Id, @event.Timestamp, @event.EventObject, token);
            return writer;
        }

        public static async Task<IEventStreamWriter> AppendAsync(this Task<IEventStreamWriter> writer, Event @event, CancellationToken token = default)
        {
            await writer.ContinueWith(async t =>
            {
                await t.Result.AppendAsync(@event);
            }, token);

            return writer.Result;
        }

        public static async Task<IEventStreamWriter> AppendAsync(this Task<IEventStreamWriter> writer, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {

            await writer.ContinueWith(async t =>
            {
                await t.AppendAsync(eventId, timestamp, @event, token);
            });

            return writer.Result;
        }

        public static async Task<IEventStreamWriter> AddRangeAsync(this Task<IEventStreamWriter> writer, IEnumerable<Event> events, CancellationToken token = default)
        {
            var result = await writer.ContinueWith(async t =>
             {
                 foreach (var @event in events)
                 {
                     await t.Result.AppendAsync(@event);
                 }
                 return t.Result;
             }, token);

            return result.Result;
        }
    }
}
