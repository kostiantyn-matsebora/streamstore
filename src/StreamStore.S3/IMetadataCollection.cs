using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.S3
{

    public interface IMetadataCollection
    {
        string this[string name] { get; }
        ICollection<string> Keys { get; }

    }
}
