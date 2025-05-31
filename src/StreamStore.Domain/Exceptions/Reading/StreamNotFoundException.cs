namespace StreamStore.Exceptions.Reading
{
    public sealed class StreamNotFoundException : ReadingException
    {
        public StreamNotFoundException(Id streamId) : base(streamId, $"Stream {streamId} is not found.")
        {
        }
    }
}