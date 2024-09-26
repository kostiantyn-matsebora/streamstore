using System;


namespace StreamDB
{

    public class UncommitedEventMetadata: IUncommitedEventMetadata
    {
        public Id Id { get;  }

        public DateTime Timestamp { get; }

        public UncommitedEventMetadata(Id id, DateTime timestamp)
        {
            if (id == Id.None) 
                throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));
            if (timestamp == default) 
                throw new ArgumentOutOfRangeException(nameof(timestamp));

            Id = id;
            Timestamp = timestamp;
        }
    }
    public class EventMetadata: UncommitedEventMetadata, IEventMetadata
    {
        public int Revision { get; }

        public EventMetadata(Id id, int revision, DateTime timestamp): base(id, timestamp)
        {
            if (revision < 1)
                throw new ArgumentOutOfRangeException("Revision cannot be less than 1", nameof(revision));

            Revision = revision;
        }
    }
}
