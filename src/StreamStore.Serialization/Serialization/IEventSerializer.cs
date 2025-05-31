namespace StreamStore.Serialization
{
    public interface IEventSerializer
    {
        byte[] Serialize(object @event);
        object Deserialize(byte[] data);
    }
}
