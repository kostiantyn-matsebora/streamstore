namespace StreamStore
{
    public sealed class StreamStoreConfiguration {
        public StreamReadingMode ReadingMode { get; set; }
        public int ReadingPageSize { get; set; }
        public bool CompressionEnabled { get; set; }
        public bool SchemaProvisioningEnabled { get; set; }
    }
}
