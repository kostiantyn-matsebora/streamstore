
namespace StreamStore.Exceptions
{
    public sealed class InvalidFromRevisionException : ReadingException
    {

        public InvalidFromRevisionException(Id streamId, Revision startFrom, Revision maxRevision) : base(streamId, "Cannot start reading from a revision greater than the current revision.")
        {
            this.StartFrom = startFrom;
            this.CurrentRevision = maxRevision;
        }

        public Revision StartFrom { get; }
        public Revision CurrentRevision { get; }
    }
}
