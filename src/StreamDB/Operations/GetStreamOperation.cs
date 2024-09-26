

using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDB.Operations
{
    internal class GetStreamOperation
    {
        readonly IEventStore store;
        readonly IEventSerializer serializer;
        Id streamId;

        public GetStreamOperation(IEventStore store, IEventSerializer serializer)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));
            this.store = store;
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            this.serializer = serializer;
        }

        public GetStreamOperation AddStreamId(Id streamId)
        {
            this.streamId = streamId;

            return this;
        }

        public async Task<Stream> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentException("streamId is required.", nameof(streamId));

            var streamEntity = 
                await store.FindAsync(streamId, cancellationToken);

            if (streamEntity == null)
                throw new StreamNotFoundException(streamId);

            return new Stream(streamEntity.Id,
                streamEntity.Events.ToStreamItemArray(serializer.Deserialize));
        }

    }
}
