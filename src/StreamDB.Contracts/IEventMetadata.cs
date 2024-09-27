using StreamDB;
using System;

internal interface IEventMetadata
{
    Id Id { get; }
    DateTime Timestamp { get; }

    int Revision { get; }
}