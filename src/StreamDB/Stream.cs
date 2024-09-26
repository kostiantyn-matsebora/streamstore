
using System.Collections.Generic;

namespace StreamDB
{

    internal sealed class Stream: IStream
    {
        readonly EventBatch<IStreamItem> stream;

        public string Id { get; }

        public IStreamItem[] Events => stream.Events;

        public int Revision => stream.MaxRevision;

        public Stream(Id id, IEnumerable<IStreamItem> events)
        {
            Id = id;

            if (events == null) 
                throw new System.ArgumentNullException(nameof(events));

            stream = new EventBatch<IStreamItem>(events);
        }
    }
}
