using System;
using System.Collections.Generic;
namespace StreamStore
{
    class StreamEventEnvelope: IStreamEventEnvelope {
        public Id Id { get; }
        public DateTime Timestamp { get; }
        public int Revision { get; }
        public object Event { get;  }

        public IReadOnlyDictionary<string,string> CustomProperties { get; } 

        public StreamEventEnvelope(Id id, Revision revision, DateTime timestamp, object @event, IReadOnlyDictionary<string,string> customProperties)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
            if (timestamp == default)
                throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            if (customProperties == null)
                throw new ArgumentNullException(nameof(customProperties));
            Id = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event;
            CustomProperties = customProperties;
        }
    }
}
