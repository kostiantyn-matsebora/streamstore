namespace StreamStore
{
    public interface IEventConverter
    {
        IStreamEventEnvelope ConvertToEnvelope(IStreamEventRecord record);

        byte[] ConvertToByteArray(object @event);
    }
}
