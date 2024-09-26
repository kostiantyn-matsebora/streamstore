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
            if (id == Id.None) throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));
            if (revision < 1) throw new ArgumentOutOfRangeException("Revision cannot be less 1", nameof(revision));
            if (timestamp == default) throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            Id = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }
    }
}
