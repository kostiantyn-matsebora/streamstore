﻿using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    sealed class StreamUnitOfWork : IStreamUnitOfWork
    {
        readonly IStreamWriter uow;
        readonly EventConverter converter;

        public StreamUnitOfWork(IStreamWriter uow, EventConverter converter)
        {
          this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
          this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public async Task<IStreamUnitOfWork> AppendEventAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
        {
            if (uow == null) throw new InvalidOperationException("Writing is not started.");

            eventId.ThrowIfHasNoValue(eventId);

            await uow!.AppendAsync(eventId, timestamp, converter.ConvertToByteArray(@event));
            return this;
        }

        public async Task<Revision> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await uow!.ComitAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
        }
    }
}
