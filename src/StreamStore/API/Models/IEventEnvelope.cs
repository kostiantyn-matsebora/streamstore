namespace StreamStore
{
    public interface IEventEnvelope: IEventMetadata
    {
        object Event { get; }
    }
}
