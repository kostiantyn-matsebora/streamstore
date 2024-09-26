using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using StreamDB.Operations;

namespace StreamDB
{
    internal class AppendToStreamOperation
    {
        readonly IEventStore store;
        readonly IEventSerializer serializer;
        Id streamId;
        IStreamItem[]? transient;
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

        public AppendToStreamOperation AddTransientEvents(IStreamItem[] transient)
        {
            this.transient = transient;
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

            if (transient == null || !transient.Any())
                throw new InvalidOperationException("There is nothing to append.");

            var streamRecord = 
                await store.FindAsync(streamId, cancellationToken);

            var revision = streamRecord?.Revision ?? 0;
            var persistent = streamRecord?.Events;

            AppentToStreamInvariants.ApplyAll(streamId, transient, persistent);

            await store.InsertAsync(streamId, transient.ToEventRecordArray(revision, serializer.Serialize), cancellationToken);

        }
    }
}
