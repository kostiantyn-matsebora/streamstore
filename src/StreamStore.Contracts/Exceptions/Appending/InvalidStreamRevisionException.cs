namespace StreamStore.Exceptions
{
    public class InvalidStreamRevisionException: ConcurrencyException
    {
        public Revision? ExpectedRevision { get; set; }
        public Revision? ActualRevision { get; set; }

        public InvalidStreamRevisionException(Id streamId, Revision expectedRevision, Revision actualRevision)
            : base(streamId, $"Expected revision {expectedRevision} does not match actual revision {actualRevision}.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
        }
    }
}
