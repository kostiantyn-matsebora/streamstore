namespace StreamStore.Exceptions
{
    public abstract class AppendingException : StreamStoreException
    {
        protected AppendingException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
