
using System.Collections.Generic;


namespace StreamStore
{
    public interface IStreamEventRecord : IStreamEventMetadata
    {
        byte[] Data { get; }

        IReadOnlyDictionary<string,string>? CustomProperties { get; }
    }
}
