
using System.Collections.Generic;

namespace StreamDB.Contracts
{
    public sealed class Stream
    {
        readonly StreamEvents<EventEnvelope> events;

        public Id Id { get; }
        public EventEnvelope[] Events => events.Events;


        public Stream(Id id, IEnumerable<EventEnvelope> events)
        {
            Id = id;
            events = new StreamEvents<EventEnvelope>(events);
        }
    }
}
