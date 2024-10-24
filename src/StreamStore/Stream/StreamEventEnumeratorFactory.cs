using System;
using System.Collections.Generic;
using System.Threading;
using StreamStore.Stream;

namespace StreamStore
{
    internal class StreamEventEnumeratorFactory
    {
        readonly StreamStoreConfiguration configuration;
        readonly IStreamReader reader;
        readonly EventConverter converter;

        public StreamEventEnumeratorFactory(StreamStoreConfiguration configuration, IStreamReader reader, EventConverter converter)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));

        }
        public IAsyncEnumerator<EventEntity> CreateEnumerator(StreamReadingParameters parameters, CancellationToken token)
        {
            if (configuration.ReadingMode == StreamReadingMode.ProduceConsume)
            {
                return new ProduceConsumeStreamEventEnumerator(parameters, reader, converter, token);
            }
             
            return new DefaultStreamEventEnumerator(parameters, reader, converter, token);
        }
    }
}