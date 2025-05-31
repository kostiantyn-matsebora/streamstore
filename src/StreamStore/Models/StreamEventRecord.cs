using System;
using System.Collections.Generic;

namespace StreamStore
{
    internal class StreamEventRecord : IStreamEventRecord
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }

        public IReadOnlyDictionary<string, string>? CustomProperties { get; set; }

        public byte[] Data { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}

