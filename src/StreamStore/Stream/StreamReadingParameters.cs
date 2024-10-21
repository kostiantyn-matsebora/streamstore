namespace StreamStore
{
    internal class StreamReadingParameters
    {
        public StreamReadingParameters(Id streamId, Revision startFrom, int pageSize)
        {
            this.StreamId = streamId;
            this.StartFrom = startFrom;
            this.PageSize = pageSize;
        }

        public Id StreamId { get; }
        public Revision StartFrom { get; }
        public int PageSize { get; }
    }
}
