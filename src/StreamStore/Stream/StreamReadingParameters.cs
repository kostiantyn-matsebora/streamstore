namespace StreamStore
{
    internal class StreamReadingParameters
    {
        public StreamReadingParameters(Id streamId, Revision startFrom, int pageSize)
        {
            StreamId = streamId;
            StartFrom = startFrom;
            PageSize = pageSize;
        }

        public Id StreamId { get; }
        public Revision StartFrom { get; }
        public int PageSize { get; }
    }
}
