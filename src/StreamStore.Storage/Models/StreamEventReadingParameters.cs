namespace StreamStore.Storage
{
    public class StreamEventReadingParameters
    {
        public Id StreamId { get; set; }
        public Revision StartFrom { get; set; }
        public int Count { get; set; }
        public int BatchSize { get; set; } = 100;
    }
}
