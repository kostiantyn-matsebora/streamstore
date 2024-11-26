namespace StreamStore.Database
{
        public enum DatabaseOperation
        {
            AppendEvent,
            DeleteStream,
            GetStreamActualRevision,
            GetStreamEventCount,
            GetEvents,
            GetStreamMetadata
        }
}
