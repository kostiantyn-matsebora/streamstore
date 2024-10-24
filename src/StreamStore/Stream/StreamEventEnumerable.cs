using System;
using System.Collections.Generic;
using System.Threading;


namespace StreamStore.Stream
{

    class StreamEventEnumerable: IAsyncEnumerable<EventEntity>
    {
        readonly StreamReadingParameters parameters;
        readonly StreamEventEnumeratorFactory enumeratorFactory;

        public StreamEventEnumerable(StreamReadingParameters parameters, StreamEventEnumeratorFactory  enumeratorFactory)
        {
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.enumeratorFactory = enumeratorFactory ?? throw new ArgumentNullException(nameof(enumeratorFactory));
        }

        public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return enumeratorFactory.CreateEnumerator(parameters, cancellationToken);
        }
    }
}
