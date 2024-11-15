
namespace StreamStore.Exceptions
{
    public abstract class ConcurrencyException : StreamStoreException
    {
        protected ConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
