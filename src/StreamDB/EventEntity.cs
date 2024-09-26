using System;
namespace StreamDB
{
    internal sealed class EventEntity: UncommitedEvent, IEventEntity
    {
        public int Revision { get; internal set; }

        public EventEntity(Id id, int revision, DateTime timestamp, object @event): base(id, timestamp, @event)
        {
            if (revision < 1) throw new ArgumentOutOfRangeException("Revision cannot be less than 1", nameof(revision));
            Revision = revision;
        }
    }
}
