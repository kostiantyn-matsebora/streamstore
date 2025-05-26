using System;
namespace StreamStore
{
    class StreamEvent: IStreamEventEnvelope {
        public Id Id { get; }
        public DateTime Timestamp { get; }
        public int Revision { get; }
        public object Event { get;  }

        public StreamEvent(Id id, Revision revision, DateTime timestamp, object @event)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
            if (timestamp == default)
                throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            Id = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event;
        }
    }
}
