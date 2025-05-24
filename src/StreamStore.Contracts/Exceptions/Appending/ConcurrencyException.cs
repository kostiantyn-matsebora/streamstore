namespace StreamStore.Exceptions
{
    public abstract class ConcurrencyException : AppendingException
    {
        protected ConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
