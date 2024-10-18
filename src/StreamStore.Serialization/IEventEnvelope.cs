namespace StreamStore.Serialization
{
    public interface IEventEnvelope
    {
        string? Type { get; }
        byte[]? Data { get; }
    }
}
