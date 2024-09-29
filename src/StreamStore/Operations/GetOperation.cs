

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.Operations
{
    internal class GetOperation
    {
        readonly IEventDatabase store;
        readonly IEventSerializer serializer;
        Id streamId;

        public GetOperation(IEventDatabase database, IEventSerializer serializer)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            this.store = database;
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            this.serializer = serializer;
        }

        public GetOperation SetStreamId(Id streamId)
        {
            this.streamId = streamId;

            return this;
        }

        public async Task<StreamEntity> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentException("streamId is required.", nameof(streamId));

            var streamRecord =
                await store.FindAsync(streamId, cancellationToken);

            if (streamRecord == null)
                throw new StreamNotFoundException(streamId);

            return ConvertToStream(streamRecord);
        }

        public StreamEntity ConvertToStream(StreamRecord record)
        {

            return new StreamEntity(record.Id,
             record.Events
                    .Select(r =>
                       new EventEntity(
                            r.Id,
                            r.Revision,
                            r.Timestamp,
                            serializer.Deserialize(r.Data!)))
                    .ToArray());
        }
    }
}
