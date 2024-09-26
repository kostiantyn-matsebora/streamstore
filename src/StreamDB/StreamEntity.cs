
using System.Collections.Generic;

namespace StreamDB
{

    internal sealed class StreamEntity: IStreamEntity
    {
        readonly EventBatch<IEventEntity> stream;

        public string Id { get; }

        public IEventEntity[] EventEntities => stream.Events;

        public int Revision => stream.MaxRevision;

        public StreamEntity(Id id, IEnumerable<IEventEntity> events)
        {
            Id = id;

            if (events == null) 
                throw new System.ArgumentNullException(nameof(events));

            stream = new EventBatch<IEventEntity>(events);
        }
    }
}
