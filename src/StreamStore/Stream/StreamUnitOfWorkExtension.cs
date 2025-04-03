using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public static class StreamUnitOfWorkExtension
    {

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> uow, IEnumerable<Event> events, CancellationToken token = default)
        {
            return uow.AppendRangeAsync(events, token).SaveChangesAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> writer, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {
            return writer.AppendEventAsync(eventId, timestamp, @event, token).SaveChangesAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> writer, Action<IEventBuilder> build, CancellationToken token = default)
        {
            return writer.AppendEventAsync(build, token).SaveChangesAsync(token);
        }

        public static Task<Revision> SaveChangesAsync(this Task<IStreamUnitOfWork> writer, CancellationToken token = default)
        {
            return FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().SaveChangesAsync(token));
        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this IStreamUnitOfWork writer, Event @event, CancellationToken token = default)
        {
            await writer.AppendEventAsync(@event.Id, @event.Timestamp, @event.EventObject, token);
            return writer;
        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this Task<IStreamUnitOfWork> writer, Event @event, CancellationToken token = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await writer.GetAwaiter().GetResult().AppendEventAsync(@event));

        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this Task<IStreamUnitOfWork> writer, Id eventId, DateTime timestamp, object @event, CancellationToken token = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await writer.GetAwaiter().GetResult().AppendEventAsync(eventId, timestamp, @event, token));

        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this IStreamUnitOfWork writer, Action<IEventBuilder> build, CancellationToken token = default)
        {
            var builder = new EventBuilder();
            build(builder);
            var @event = builder.Build();

            await writer.AppendEventAsync(@event, token);
            return writer;
        }

        public static Task<IStreamUnitOfWork> AppendEventAsync(this Task<IStreamUnitOfWork> writer, Action<IEventBuilder> build, CancellationToken token = default)
        {
            return FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().AppendEventAsync(build, token));
        }

        public static async Task<IStreamUnitOfWork> AppendRangeAsync(this Task<IStreamUnitOfWork> writer, IEnumerable<Event> events, CancellationToken token = default)
        {
            foreach (var @event in events)
            {
                await FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().AppendEventAsync(@event));
            }
            return writer.Result;
        }
    }
}
