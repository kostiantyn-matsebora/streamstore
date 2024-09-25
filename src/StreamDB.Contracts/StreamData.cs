using System;


namespace StreamDB
{
    public sealed class StreamData
    {
        public string Id { get; }

        public int Revision { get; }

        public EventData[] Events { get; }

        public StreamData(string id, int revision, EventData[] events)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Id = id;
            if (revision <= 0)
                throw new ArgumentException("Invalid revision, must be greater than 0", nameof(revision));
            Revision = revision;
            if (events == null || events.Length == 0)
                throw new ArgumentException("Events must contain at least one event", nameof(events));

            Events = events;

        }
    }
}
