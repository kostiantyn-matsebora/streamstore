using StreamStore.Models;

namespace StreamStore
{
    public interface IStreamEventMetadata: IEventMetadata, IHasRevision
    {
    }
}
