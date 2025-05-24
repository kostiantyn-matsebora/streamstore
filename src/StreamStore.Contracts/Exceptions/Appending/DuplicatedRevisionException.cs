namespace StreamStore.Exceptions
{
    public sealed class DuplicatedRevisionException : ValidationException
    {
        public Revision? Revision { get; }

        public DuplicatedRevisionException(Revision revision, Id streamId) : 
            base(streamId, $"Duplicate revision {revision} for stream {streamId}")
        {
            Revision = revision;
        }

        public DuplicatedRevisionException(Id streamId) :
           base(streamId, $"Duplicate revision for stream {streamId}")
        {
        }
    }
}
