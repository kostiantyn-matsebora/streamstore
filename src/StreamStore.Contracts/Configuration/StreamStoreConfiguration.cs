namespace StreamStore
{
    public sealed class StreamStoreConfiguration {
        public StreamReadingMode Mode { get; set; }
        public int ReadingPageSize { get; set; }
        public bool Compression { get; set; }
    }
}
