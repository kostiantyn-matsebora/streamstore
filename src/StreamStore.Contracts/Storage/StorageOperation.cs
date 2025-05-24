namespace StreamStore.Storage
{
        public enum StorageOperation
        {
            AppendEvent,
            DeleteStream,
            GetStreamMetadata,
            GetStreamEventCount,
            GetEvents,
            GetStreamEventsMetadata
        }
}
