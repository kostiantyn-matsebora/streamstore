namespace StreamStore.Exceptions.Appending
{
    public abstract class ValidationException : AppendingException
    {
        protected ValidationException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
