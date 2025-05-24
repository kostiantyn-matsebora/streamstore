namespace StreamStore.Exceptions
{
    public class StreamNotFoundException : ReadingException
    {
        public StreamNotFoundException(Id streamId) : base(streamId, $"Stream {streamId} is not found.")
        {
        }
    }
}