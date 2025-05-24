namespace StreamStore.Exceptions
{
    public class StreamNotFoundException : StreamStoreException
    {
        public StreamNotFoundException(Id streamId) : base(streamId, $"Stream {streamId} is not found.")
        {
        }
    }
}