using System;
namespace StreamDB
{
    public class UncommitedEvent : UncommitedEventMetadata
    {
        public object Event { get;  }

        public UncommitedEvent(Id id, DateTime timestamp, object @event) : base(id, timestamp)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            Event = @event;
        }
    }
}
