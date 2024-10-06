
using System.Collections.Generic;

namespace StreamStore.S3.Client
{
    public interface  IS3ReadonlyMetadataCollection
    {
        string this[string key] { get; }
        ICollection<string> Keys { get; }
    }
}
