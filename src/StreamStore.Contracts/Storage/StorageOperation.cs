namespace StreamStore.Storage
{
        public enum StorageOperation
        {
            AppendEvent,
            DeleteStream,
            GetStreamActualRevision,
            GetStreamEventCount,
            GetEvents,
            GetStreamMetadata
        }
}
