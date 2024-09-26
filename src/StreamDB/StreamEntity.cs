
using System.Collections.Generic;

namespace StreamDB
{

    public sealed class StreamEntity
    {
        readonly EventBatch<EventEntity> batch;

        public string Id { get; }

        public EventEntity[] EventEntities => batch.Events;

        public int Revision => batch.MaxRevision;

        public StreamEntity(Id id, IEnumerable<EventEntity> events)
        {
            Id = id;

            if (events == null) 
                throw new System.ArgumentNullException(nameof(events));

            batch = new EventBatch<EventEntity>(events);
        }
    }
}
