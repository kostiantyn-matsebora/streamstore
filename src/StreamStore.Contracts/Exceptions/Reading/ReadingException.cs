namespace StreamStore.Exceptions { 
    public abstract class ReadingException : StreamStoreException
    {
        protected ReadingException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
