﻿using System;


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
