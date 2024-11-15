namespace StreamStore
{
    public sealed class StreamStoreConfiguration {
        public StreamReadingMode ReadingMode { get; set; }
        public int ReadingPageSize { get; set; }
    }
}
