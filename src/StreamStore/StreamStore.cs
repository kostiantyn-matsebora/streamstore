﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Operations;


namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IEventTable store;
        readonly IEventSerializer serializer;

        public StreamStore(IEventTable store) : this(store, new EventSerializer())
        {
            this.store = store;
            serializer = new EventSerializer();
        }

        public StreamStore(IEventTable store, IEventSerializer serializer)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.store = store;
            this.serializer = serializer;
        }

        public async Task AppendAsync(string streamId, IEnumerable<UncommitedEvent> uncommited, int expectedRevision, CancellationToken cancellationToken = default)
        {
            await new AppendOperation(store, serializer)
                 .AddStreamId(streamId)
                 .AddUncommitedEvents(uncommited)
                 .AddExpectedRevision(expectedRevision)
                 .ExecuteAsync(cancellationToken);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken = default)
        {
            await new DeleteOperation(store)
                     .AddStreamId(streamId)
                     .ExecuteAsync(cancellationToken);
        }

        public async Task<StreamEntity> GetAsync(string streamId, CancellationToken cancellationToken = default)
        {
            return
                await new GetOperation(store, serializer)
                 .AddStreamId(streamId)
                 .ExecuteAsync(cancellationToken);
        }
    }
}
