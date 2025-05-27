using StreamStore.Models;

namespace StreamStore
{
    public interface IEventEnvelope: IEventMetadata
    {
        object Event { get; }
        ICustomProperties CustomProperties { get; }
    }
}
