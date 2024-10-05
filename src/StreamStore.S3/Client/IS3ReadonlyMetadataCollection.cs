
using System.Collections.Generic;


namespace StreamStore.S3.Client
{

    public interface IS3ReadonlyMetadataCollection
    {
        string this[string name] { get; }
        ICollection<string> Keys { get; }

    }
}
