using System;

namespace StreamStore
{
    public interface IStreamMetadata
    {
        Id Id { get; }
        Revision Revision { get; }

        DateTime LastModified { get; }
    }
}
