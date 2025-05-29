using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StreamStore.NoSql.Cassandra.Models
{
    [ExcludeFromCodeCoverage]
    internal class EventEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Id { get; set; }
        public string StreamId { get; set; }
        public int Revision { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[]? Data { get; set; }
        public IDictionary<string, string> CustomProperties { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
