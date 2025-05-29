using System.Collections.Generic;

namespace StreamStore
{
    public interface IStreamEventEnvelope: IEventMetadata, IHasRevision
    {
        object Event { get; }
        public IReadOnlyDictionary<string, string> CustomProperties { get; }
    }
}
