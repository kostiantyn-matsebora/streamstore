using System;
using StreamDB;

namespace StreamDB
{
    public sealed class EventRecord: IEventMetadata
    {
        public Id Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Revision { get; set; }
        public string? Data { get; set; }
    }
}

public interface IEventMetadata
{
    Id Id { get; }
    DateTime Timestamp { get; }
    int Revision { get; }
}