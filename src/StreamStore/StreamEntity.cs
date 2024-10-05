
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore
{

    public sealed class StreamEntity
    {
        public Id StreamId { get; }

        public EventEntityCollection EventEntities { get; }

        public int Revision => EventEntities.MaxRevision;

        internal StreamEntity(Id id, IEnumerable<EventEntity> events)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");

            StreamId = id;
            
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            EventEntities = new EventEntityCollection(events);
        }
    }

    public sealed class EventEntityCollection : RevisionedItemCollection<EventEntity>
    {
        public EventEntityCollection(IEnumerable<EventEntity> items) : base(items)
        {
        }
    }
}
