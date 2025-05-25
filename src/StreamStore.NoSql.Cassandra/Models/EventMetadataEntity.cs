using System;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.NoSql.Cassandra.Models
{
    [ExcludeFromCodeCoverage]
    internal class EventMetadataEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Id { get; set; }
        public string StreamId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public int Revision { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
