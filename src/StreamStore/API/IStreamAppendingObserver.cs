namespace StreamStore.API
{
    public interface IStreamAppendingObserver
    {
        void OnBeginAppend(Id streamId, Revision expectedRevision);
        void OnEventAppended(Id streamId, Revision revision, IEventEnvelope envelope);
        void OnAppendSucceeded(Id streamId, Revision revision, int eventCount);
    }
}
