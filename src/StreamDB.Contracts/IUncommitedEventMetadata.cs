using System;
using StreamDB;

public interface IUncommitedEventMetadata
{
    Id Id { get; }
    DateTime Timestamp { get; }
}


