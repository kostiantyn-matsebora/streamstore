using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;


namespace StreamStore
{
    sealed class EventStreamReader : IEventStreamReader
    {
        readonly StreamReadingParameters parameters;
        readonly StreamEventProducer producer;
        Task? producerTask;

        public EventStreamReader(StreamReadingParameters parameters, StreamEventProducer producer)
        {
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return producer.StartProducing(parameters, cancellationToken).GetAsyncEnumerator();
        }

        public async Task<EventEntityCollection> ReadToEndAsync(CancellationToken cancellationToken = default)
        {

            var results = new List<EventEntity>();
            await foreach (var item in producer.StartProducing(parameters, cancellationToken))
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
