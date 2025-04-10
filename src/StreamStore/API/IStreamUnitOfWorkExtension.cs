using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace StreamStore
{
    public static class IStreamUnitOfWorkExtension
    {

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> uow, IEnumerable<IEventEnvelope> events, CancellationToken token = default)
        {
            return uow.AppendRangeAsync(events, token).SaveChangesAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> writer, IEventEnvelope envelope, CancellationToken token = default)
        {
            return writer.AppendEventAsync(envelope, token).SaveChangesAsync(token);
        }

        public static Task<Revision> WriteAsync(this Task<IStreamUnitOfWork> writer, Action<IEventEnvelopeBuilder> build, CancellationToken token = default)
        {
            return writer.AppendEventAsync(build, token).SaveChangesAsync(token);
        }

        public static Task<Revision> SaveChangesAsync(this Task<IStreamUnitOfWork> writer, CancellationToken token = default)
        {
            return FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().SaveChangesAsync(token));
        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this IStreamUnitOfWork writer, IEventEnvelope envelope, CancellationToken token = default)
        {
            await writer.AppendAsync(envelope, token);
            return writer;
        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this Task<IStreamUnitOfWork> writer, IEventEnvelope envelope, CancellationToken token = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await writer.GetAwaiter().GetResult().AppendAsync(envelope));

        }

        public static async Task<IStreamUnitOfWork> AppendEventAsync(this IStreamUnitOfWork writer, Action<IEventEnvelopeBuilder> build, CancellationToken token = default)
        {
            var builder = new EventEnvelopeBuilder();
            build(builder);
            var @event = builder.Build();

            await writer.AppendAsync(@event, token);
            return writer;
        }

        public static Task<IStreamUnitOfWork> AppendEventAsync(this Task<IStreamUnitOfWork> writer, Action<IEventEnvelopeBuilder> build, CancellationToken token = default)
        {
            return FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().AppendEventAsync(build, token));
        }

        public static async Task<IStreamUnitOfWork> AppendRangeAsync(this Task<IStreamUnitOfWork> writer, IEnumerable<IEventEnvelope> events, CancellationToken token = default)
        {
            foreach (var @event in events)
            {
                await FuncExtension.ThrowOriginalExceptionIfOccured(() => writer.GetAwaiter().GetResult().AppendAsync(@event));
            }
            return writer.Result;
        }
    }
}
