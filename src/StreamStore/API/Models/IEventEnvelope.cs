using System.Collections.Generic;
namespace StreamStore
{
    public interface IEventEnvelope: IEventMetadata
    {
        object Event { get; }
        IReadOnlyDictionary<string,string>? CustomProperties { get; }
    }
}
