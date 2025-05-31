namespace StreamStore.S3.Tests.Storage
{
    class StreamEventMetadata : IStreamEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }

        public IReadOnlyDictionary<string, string>? CustomProperties { get; set; }
    }
}
