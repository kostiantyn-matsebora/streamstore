
namespace StreamStore.Exceptions
{
    public class DuplicateRevisionException : StreamStoreException
    {
        public Revision Revision { get; }
        public DuplicateRevisionException(Revision revision, Id streamId) : 
            base(streamId, $"Duplicate revision {revision} for stream {streamId}")
        {
            Revision = revision;
        }
    }
}
