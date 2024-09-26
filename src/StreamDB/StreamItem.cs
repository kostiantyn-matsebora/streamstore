using System;
namespace StreamDB
{
    public sealed class StreamItem: IStreamItem
    {
        public Id Id { get; internal set; }

        public DateTime Timestamp { get; internal set; }

        public int Revision { get; internal set; }

        public object Event { get; internal set; }

        public StreamItem(Id id, int revision, DateTime timestamp, object @event)
        {
            if (revision < 1) throw new ArgumentOutOfRangeException(nameof(revision));

            Id = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }
    }
}
