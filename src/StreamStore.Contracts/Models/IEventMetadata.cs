using System;

namespace StreamStore
{
    public interface IEventMetadata
    {
        Id Id { get;  }
        DateTime Timestamp { get; }
    }
}
