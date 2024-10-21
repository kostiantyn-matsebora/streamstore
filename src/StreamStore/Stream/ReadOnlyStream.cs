using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;


namespace StreamStore
{
    sealed class ReadOnlyStream : IReadOnlyStream
    {
        readonly StreamReadingParameters parameters;
        readonly StreamEventProducerFactory producerFactory;
        readonly StreamEventQueue queue;
        Task? producerTask;

        public ReadOnlyStream(StreamReadingParameters parameters, StreamEventProducerFactory producerFactory, StreamEventQueue queue)
        {
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.producerFactory = producerFactory ?? throw new ArgumentNullException(nameof(producerFactory));
            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public IReadOnlyStream BeginRead(CancellationToken token)
        {
            if (producerTask != null)
            {
                throw new InvalidOperationException("Already reading a stream.");
            }
            producerTask = producerFactory.StartProducing(parameters, queue, token);
            return this;
        }

        public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return queue.ReadAsync(cancellationToken).GetAsyncEnumerator();
        }

        public async Task<EventEntityCollection> ReadToEndAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<EventEntity>();
            await foreach (var item in queue.ReadAsync())
            {
                results.Add(item);
            }
            return new EventEntityCollection(results);
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
                producerTask?.Dispose();
            }
        }
    }
}
