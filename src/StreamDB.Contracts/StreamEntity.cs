using System;
using System.Collections.Generic;
namespace StreamDB
{
    public sealed class StreamEntity
    {
        readonly StreamEvents<EventEntity> events;

        public Id Id { get; }

        public int Revision => events.MaxRevision.GetValueOrDefault();

        public EventEntity[] Events => events.Events;

        public StreamEntity(Id id, IEnumerable<EventEntity> events)
        {
            Id = id;
            this.events = new StreamEvents<EventEntity>(events);
        }
    }
}
