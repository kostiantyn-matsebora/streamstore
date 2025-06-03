namespace StreamStore
{
    public sealed class StreamStoreConfiguration {
        public StreamReadingMode ReadingMode { get; set; } = StreamReadingMode.Default;
        public int ReadingPageSize { get; set; } = 1000;
    }
}
