using System;
namespace StreamDB
{
    public sealed class EventEnvelope: IHasRevision
    {
        public Id Id { get; internal set; }

        public DateTime Timestamp { get; internal set; }

        public int Revision { get; internal set; }

        public object Event { get; internal set; }

        public EventEnvelope(Id id, int revision, DateTime timestamp, object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (revision < 1) throw new ArgumentOutOfRangeException(nameof(revision));

            Id = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event;
        }
    }
}
