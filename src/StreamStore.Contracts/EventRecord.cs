using System;


namespace StreamStore
{
    public sealed class EventRecord : EventMetadataRecord
    {
        public string? Data { get; set; }
    }
}

