using System;
using StreamDB;

public interface IEventMetadata
{
    Id Id { get; }
    DateTime Timestamp { get; }
    int Revision { get; }
}


