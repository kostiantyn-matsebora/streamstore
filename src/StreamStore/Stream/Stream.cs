using System;
using StreamStore.Exceptions;
using System.Threading.Tasks;
using System.Threading;



namespace StreamStore
{
    internal class Stream: IStream
    {
        readonly EventConverter converter;
        readonly StreamEventProducerFactory producerFactory;
        private readonly StreamContext ctx;
        readonly IStreamDatabase database;

        public Stream(StreamContext ctx, IStreamDatabase database, EventConverter converter, StreamEventProducerFactory producerFactory)
        {
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            this.producerFactory = producerFactory ?? throw new ArgumentNullException(nameof(producerFactory));
        }

        public async Task<IEventStreamWriter> BeginWriteAsync(Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            if (expectedRevision != ctx.CurrentRevision)
                throw new OptimisticConcurrencyException(expectedRevision, ctx.CurrentRevision, ctx.StreamId);

            var uow = await database.BeginAppendAsync(ctx.StreamId, expectedRevision);

            if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");

            return new EventStreamWriter(uow, converter);

        }

  
        public IEventStreamReader BeginRead(Revision startFrom, CancellationToken cancellationToken = default)
        {
            if (ctx.CurrentRevision < startFrom)
                throw new InvalidOperationException("Cannot start reading from a revision greater than the current revision.");

            var parameters = new StreamReadingParameters(ctx.StreamId, startFrom, ctx.PageSize);
           
            var queue = new StreamEventQueue(ctx.PageSize);
           
            return new EventStreamReader(parameters, producerFactory, queue).BeginRead(cancellationToken);
        }
    }
}
