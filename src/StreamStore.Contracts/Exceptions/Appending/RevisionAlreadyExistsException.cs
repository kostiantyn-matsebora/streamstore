namespace StreamStore.Exceptions
{
    public class RevisionAlreadyExistsException : OptimisticConcurrencyException
    {
        public Revision? Revision { get; }

        public RevisionAlreadyExistsException(Revision revision, Id streamId) : 
            base(streamId, $"Duplicate revision {revision} for stream {streamId}")
        {
            Revision = revision;
        }

        public RevisionAlreadyExistsException(Id streamId) :
           base(streamId, $"Duplicate revision for stream {streamId}")
        {
        }
    }
}
