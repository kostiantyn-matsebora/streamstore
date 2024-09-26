using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamDB.Contracts;


namespace StreamDB
{
    public sealed class StreamDB: IStreamDB
    {
        readonly IEventStore store;
        readonly IEventSerializer serializer;

        public StreamDB(IEventStore store) : this(store, new EventSerializer())
        {
        }

        public StreamDB(IEventStore store, IEventSerializer serializer)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.store = store;
            this.serializer = serializer;
        }

        public async Task SaveAsync(string streamId, EventEnvelope[] uncommited, int expectedRevision, CancellationToken ct = default)
        {
            if (streamId == null) throw new ArgumentNullException(nameof(streamId));

            var streamEntity = await store.FindAsync(streamId, ct);
            var revision = streamEntity?.Revision ?? 0;

            var uncommitedEvents = new StreamEvents<EventEntity>(uncommited.Select(e => e.ToEventEntity(revision++, serializer.Serialize)).ToArray());
          
            await store.InsertAsync(streamId, uncommitedEvents, ct);
        }

        public async Task DeleteAsync(string streamId, CancellationToken ct = default)
        {
            if (streamId == null) throw new ArgumentNullException(nameof(streamId));
            
            await store.DeleteAsync(streamId, ct);
        }

        public async Task<Stream> GetAsync(string streamId, CancellationToken ct = default)
        {
            var streamEntity = await store.FindAsync(streamId, ct);
            if (streamEntity == null) 
                throw new StreamNotFoundException(streamId);

            return new Stream(
                streamEntity.Id, 
                new StreamEvents<EventEnvelope>(streamEntity.Events.Select(e => e.ToEvent(serializer.Deserialize)))
                .ToArray());
        }
    }
}
