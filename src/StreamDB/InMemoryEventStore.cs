using StreamDB;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading;
using System.Linq;
using System.Collections.Concurrent;

namespace StreamDB
{

    public class InMemoryEventStore : IEventStore
    {
        readonly ConcurrentDictionary<string, StreamEventEntities> eventStore = new ConcurrentDictionary<string, StreamEventEntities>();

        public Task<StreamEntity> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            if (!eventStore.TryGetValue(streamId, out var eventEntities))
                return null;

            return Task.FromResult(new StreamEntity(streamId, eventEntities));

        }

        public Task InsertAsync(string streamId, IEnumerable<EventEntity> uncommited, CancellationToken cancellationToken)
        {
            var uncommitedEntities = new StreamEventEntities(uncommited);

            if (!eventStore.TryGetValue(streamId, out var commited))
            {
                if (!eventStore.TryAdd(streamId, uncommitedEntities))  // It seems like stream has been already added
                {
                    commited = eventStore[streamId];
                    throw new OptimisticConcurrencyException(0, commited.Count(), streamId);
                }
                return Task.CompletedTask;
            }

            StreamEventsValidator<EventEntity,EventEntity>.Validate(uncommitedEntities, commited, streamId);

            commited.AddRange(uncommited);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            if (eventStore.ContainsKey(streamId))
                eventStore.TryRemove(streamId, out var commited);

            return Task.CompletedTask;
        }

        class StreamEventEntities : StreamEvents<EventEntity>
        {
            public StreamEventEntities(IEnumerable<EventEntity> events) : base(events)
            {
            }
        }
    }
}