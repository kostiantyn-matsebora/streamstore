namespace StreamStore.Exceptions.Appending
{
    public abstract class AppendingException : StreamStoreException
    {
        protected AppendingException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
