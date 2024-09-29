
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore
{

    public sealed class StreamEntity
    {
       
        public string StreamId { get; }

        public EventEntity[] EventEntities;

        public int Revision { get; }

        public StreamEntity(Id id, IEnumerable<EventEntity> events)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));

            StreamId = id;
            
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            EventEntities = events.OrderBy(e => e.Revision).ToArray();
            Revision = EventEntities.Any() ? EventEntities.Max(e => e.Revision): 0;
        }
    }
}
