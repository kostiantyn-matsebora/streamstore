namespace StreamStore.Storage
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
