using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public static class StreamExtension
    {
        public static Task<Revision> SaveChangesAsync(this Task<IWriteOnlyStream> stream, CancellationToken token)
        {
            return stream.Result!.SaveChangesAsync(token);
        }

        public static async Task<IWriteOnlyStream> AddAsync(this IWriteOnlyStream stream, Event @event, CancellationToken token = default)
        {
            await stream.AddAsync(@event.Id, @event.Timestamp, @event.EventObject, token);
            return stream;
        }

        public static async Task<IWriteOnlyStream> AddAsync(this Task<IWriteOnlyStream> stream, Event @event, CancellationToken token = default)
        {
            await stream.ContinueWith(async t =>
            {
                await t.Result.AddAsync(@event);
            }, token);

            return stream.Result;
        }

        public static async Task<IWriteOnlyStream> AddAsync(this Task<IWriteOnlyStream> stream, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {

            await stream.ContinueWith(async t =>
            {
                await t.AddAsync(eventId, timestamp, @event, token);
            });

            return stream.Result;
        }

        public static async Task<IWriteOnlyStream> AddRangeAsync(this Task<IWriteOnlyStream> stream, IEnumerable<Event> events, CancellationToken token = default)
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

        public static IWriteOnlyStream AddRange(this IWriteOnlyStream stream, IEnumerable<Event> events)
        {

            IWriteOnlyStream result = stream;
            foreach (var @event in events)
            {
                result = stream.AddAsync(@event).GetAwaiter().GetResult();
            }

            return result;
        }

        public static IWriteOnlyStream Add(this IWriteOnlyStream stream, Id eventId, DateTime timestamp, object @event)
        {
            return stream.AddAsync(eventId, timestamp, @event).GetAwaiter().GetResult();
        }

    }
}
