using System;
using StreamStore.Models;

namespace StreamStore
{
    public interface IEventMetadata
    {
        Id Id { get;  }
        DateTime Timestamp { get; }

        public ICustomProperties CustomProperties { get; }
    }
}
