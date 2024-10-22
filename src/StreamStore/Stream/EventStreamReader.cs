using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using StreamStore.Stream;


namespace StreamStore
{
    sealed class EventStreamReader : IEventStreamReader
    {
        readonly StreamReadingParameters parameters;
        private readonly IStreamDatabase database;
        private readonly EventConverter converter;

        public EventStreamReader(StreamReadingParameters parameters, IStreamDatabase database, EventConverter converter)
        {
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var enumerable = new EventStreamEnumerable(parameters, database, converter);

            return enumerable.GetAsyncEnumerator(cancellationToken);
        }

        public async Task<EventEntityCollection> ReadToEndAsync(CancellationToken cancellationToken = default)
        {

            var results = new List<EventEntity>();

            await foreach (var item in new EventStreamEnumerable(parameters, database, converter))
            {
                results.Add(item);
            }
            return new EventEntityCollection(results);
        }

     
    }
}
