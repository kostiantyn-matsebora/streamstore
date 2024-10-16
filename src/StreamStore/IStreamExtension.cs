using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public static class IStreamExtension
    {
        public static Task<Revision> SaveChangesAsync(this Task<IStream> stream, CancellationToken token)
        {
            return stream.Result!.SaveChangesAsync(token);
        }

        public static async Task<IStream> AddAsync(this IStream stream, Event @event, CancellationToken token = default)
        {
            await stream.AddAsync(@event.Id, @event.Timestamp, @event.EventObject, token);
            return stream;
        }

        public static async Task<IStream> AddAsync(this Task<IStream> stream, Event @event, CancellationToken token = default)
        {
            await stream.ContinueWith(async t =>
            {
                await t.Result.AddAsync(@event);
            }, token);

            return stream.Result;
        }

        public static async Task<IStream> AddAsync(this Task<IStream> stream, Id eventId, DateTime timestamp, object @event, CancellationToken token = default) {

            await stream.ContinueWith(async t =>
            {
                await t.AddAsync(eventId, timestamp, @event, token);
            });

            return stream.Result;
        }

        public static async Task<IStream> AddRangeAsync(this Task<IStream> stream, IEnumerable<Event> events, CancellationToken token = default)
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

        public static IStream AddRange(this IStream stream, IEnumerable<Event> events)
        {

            IStream result = stream;
            foreach (var @event in events)
            {
              result = stream.AddAsync(@event).GetAwaiter().GetResult();
            }

            return result;
        }

        public static IStream Add(this IStream stream, Id eventId, DateTime timestamp, object @event)
        {
           return stream.AddAsync(eventId, timestamp, @event).GetAwaiter().GetResult();
        }
    }
}
