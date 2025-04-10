using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    class StreamUnitOfWork : IStreamUnitOfWork
    {
        readonly IStreamWriter writer;
        readonly EventConverter converter;

        public StreamUnitOfWork(IStreamWriter writer, EventConverter converter)
        {
          this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
          this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public async Task<IStreamUnitOfWork> AppendAsync(IEventEnvelope envelope, CancellationToken cancellationToken = default)
        {
            if (writer == null) throw new InvalidOperationException("Writing is not started.");
            envelope.ThrowIfNull(nameof(envelope));

            await writer!.AppendAsync(envelope.Id, envelope.Timestamp, converter.ConvertToByteArray(envelope.Event));
            return this;
        }

        public async Task<Revision> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await writer!.ComitAsync(cancellationToken);
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
                writer.Dispose();
            }
        }
    }
}
