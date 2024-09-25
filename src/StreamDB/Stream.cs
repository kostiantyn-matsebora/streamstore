using System;


namespace StreamDB.Contracts
{
    public sealed class Stream {
        public Id Id { get; }
        public int Revision { get; }
        public EventEnvelope[] Events { get; }


        public Stream(Id id, int revision, EventEnvelope[] events)
        {
            if (revision <= 0)
                throw new ArgumentException("Invalid revision, must be greater than 0", nameof(revision));
            Revision = revision;
            if (events == null || events.Length == 0)
                throw new ArgumentException("Events must contain at least one event", nameof(events));
        }
    }
}
