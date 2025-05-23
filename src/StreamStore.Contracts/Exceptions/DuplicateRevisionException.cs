
namespace StreamStore.Exceptions
{
    public class DuplicateRevisionException : ConcurrencyException
    {
        public Revision? Revision { get; }
        public DuplicateRevisionException(Revision revision, Id streamId) : 
            base(streamId, $"Duplicate revision {revision} for stream {streamId}")
        {
            Revision = revision;
        }

        public DuplicateRevisionException(Id streamId) :
           base(streamId, $"Duplicate revision for stream {streamId}")
        {
        }
    }
}
