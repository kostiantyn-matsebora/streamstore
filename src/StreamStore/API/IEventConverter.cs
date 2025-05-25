namespace StreamStore
{
    public interface IEventConverter
    {
        IStreamEvent ConvertToEvent(IStreamEventRecord record);
        byte[] ConvertToByteArray(object @event);
    }
}
