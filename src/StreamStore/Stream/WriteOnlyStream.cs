using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    sealed class WriteOnlyStream : IWriteOnlyStream
    {
        readonly IStreamUnitOfWork uow;
        readonly EventConverter converter;

        public WriteOnlyStream(IStreamUnitOfWork uow, EventConverter converter)
        {
          this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
          this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public async Task<IWriteOnlyStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
        {
            if (uow == null)
                throw new InvalidOperationException("Writing is not started.");

            if (eventId == Id.None)
                throw new ArgumentNullException(nameof(eventId));

            await uow!.AddAsync(eventId, timestamp, converter.ConvertToByteArray(@event));
            return this;
        }

        public async Task<Revision> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await uow!.SaveChangesAsync(cancellationToken);
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
