using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public static class IStreamExtension
    {
        public static Task SaveChangesAsync(this Task<IStream> stream, CancellationToken token)
        {
            return stream.Result!.SaveChangesAsync(token);
        }

        public static Task AddAsync(this Task<IStream> stream, Id eventId, DateTime timestamp, object @event)
        {
            stream.Result!.Add(eventId, timestamp, @event);
            return Task.CompletedTask;
        }

        public static IStream Add(this IStream stream, Event @event)
        {
            stream.Add(@event.Id, @event.Timestamp, @event.EventObject);
            return stream;
        }

        public static Task<IStream> AddRangeAsync(this Task<IStream> stream, IEnumerable<Event> events)
        {
            var streamResult = stream.Result!;
            foreach (var @event in events)
            {
                streamResult.Add(@event);
            }
            return Task.FromResult(streamResult);
        }

        public static IStream AddRange(this IStream stream, IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                stream.Add(@event.Id, @event.Timestamp, @event.EventObject);
            }
            return stream;
        }
    }
}
