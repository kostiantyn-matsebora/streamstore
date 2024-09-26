﻿using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using StreamDB.Operations;
using System.Collections.Generic;


namespace StreamDB
{
    internal class AppendToStreamOperation
    {
        readonly IEventStore store;
        readonly IEventSerializer serializer;
        Id streamId;
        UncommitedEvent[]? uncommited;
        int expectedRevision;

        public AppendToStreamOperation(IEventStore store, IEventSerializer serializer)
        {
            if(store == null)
                throw new ArgumentNullException(nameof(store));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            this.store = store;
            this.serializer = serializer;
        }

        public AppendToStreamOperation AddStreamId(Id streamId)
        {
            this.streamId = streamId;
            return this;
        }

        public AppendToStreamOperation AddUncommitedEvents(IEnumerable<UncommitedEvent> uncommited)
        {
            this.uncommited = uncommited.ToArray();
            return this;
        }

        public AppendToStreamOperation AddExpectedRevision(int expectedRevision)
        {
            this.expectedRevision = expectedRevision;
            return this;
        }

        public async Task  ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentException("streamId is required.", nameof(streamId));

            if (uncommited == null || !uncommited.Any())
                throw new InvalidOperationException("There is nothing to append.");

            var streamRecord = 
                await store.FindAsync(streamId, cancellationToken);

            var revision = streamRecord?.Revision ?? 0;
            var persistent = streamRecord?.Events;
            
            var transient = ToEventEntity(uncommited, revision);

            AppentToStreamInvariants.CheckAll(streamId, transient, persistent);
            var eventRecordBatch = ToEventRecordBatch(transient, revision);

            await store.InsertAsync(streamId, eventRecordBatch, cancellationToken);

        }

        EventEntity[] ToEventEntity(UncommitedEvent[] items, int revision)
        {
            return items.
                Select(e => new EventEntity(e.Id, revision++, e.Timestamp, e.Event))
                .ToArray();
        }

        EventRecordBatch ToEventRecordBatch(EventEntity[] items, int revision)
        {
            return new EventRecordBatch(
                items
                   .Select(e => 
                        new EventRecord
                            {
                                Id = e.Id,
                                Timestamp = e.Timestamp,
                                Revision = revision++,
                                Data = serializer.Serialize(e.Event)
                            })
                    .ToArray());
        }

    }
}
