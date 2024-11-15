namespace StreamStore
{
    public interface IEventSerializer
    {
        byte[] Serialize(object @event);
        object Deserialize(byte[] data);
    }
}
