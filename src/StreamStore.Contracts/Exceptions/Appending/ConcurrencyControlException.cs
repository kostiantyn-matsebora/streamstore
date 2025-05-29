namespace StreamStore.Exceptions
{
    public abstract class ConcurrencyControlException : AppendingException
    {
        protected ConcurrencyControlException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
