using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.API;
using StreamStore.Operations;
using StreamStore.Serialization;


namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IEventDatabase store;
        readonly IEventSerializer serializer;

        public StreamStore(IEventDatabase store) : this(store, new EventSerializer())
        {
        }

        public StreamStore(IEventDatabase store, IEventSerializer serializer)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.store = store;
            this.serializer = serializer;
        }

        public async Task<IStream> OpenStreamAsync(string streamId, int expectedRevision, CancellationToken cancellationToken = default)
        {
            var operation = new AppendOperation(store, serializer);
            await operation.OpenAsync(streamId, expectedRevision, cancellationToken);
            return operation;
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken = default)
        {
            await new DeleteOperation(store)
                     .SetStreamId(streamId)
                     .ExecuteAsync(cancellationToken);
        }

        public async Task<StreamEntity> GetAsync(string streamId, CancellationToken cancellationToken = default)
        {
            return
                await new GetOperation(store, serializer)
                 .SetStreamId(streamId)
                 .ExecuteAsync(cancellationToken);
        }
    }
}
