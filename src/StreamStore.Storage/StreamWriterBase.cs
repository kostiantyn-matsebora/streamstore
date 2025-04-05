using StreamStore.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;


namespace StreamStore.Storage
{
    public abstract class StreamWriterBase : IStreamWriter
    {
        protected readonly Id streamId;
        protected readonly Revision expectedRevision;
        readonly EventRecordCollection events = new EventRecordCollection();
        readonly EventRecordCollection uncommited = new EventRecordCollection();

        Revision revision;

        protected StreamWriterBase(Id streamId, Revision expectedRevision, EventRecordCollection? existing)
        {
            this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));

            if (existing != null && existing.Any())
            {
                events.AddRange(existing);
            }

            this.expectedRevision = expectedRevision;
            revision = expectedRevision;
            
        }

        public async Task<Revision> ComitAsync(CancellationToken cancellationToken)
        {
            ThrowIfThereIsDuplicates();

            await CommitAsync(uncommited, cancellationToken);
            return events.MaxRevision;
        }

        public async Task<IStreamWriter> AppendAsync(Id eventId, DateTime timestamp, byte[] data, CancellationToken token = default)
        {
            eventId.ThrowIfHasNoValue(nameof(eventId));
            timestamp.ThrowIfMinValue(nameof(timestamp));
            data.ThrowIfNull(nameof(data));


            ThrowIfDuplicateEventId(eventId);

            // Since revision is immutable, we need to assign the new value to revision
            revision = revision.Increment();
            
            var eventRecord = new EventRecord
            {
                Id = eventId,
                Revision = revision,
                Timestamp = timestamp,
                Data = data
            };

            events!.Add(eventRecord);
            uncommited.Add(eventRecord);
            await OnEventAdded(eventRecord, token);

            return this;
        }

        protected abstract Task CommitAsync(EventRecordCollection uncommited, CancellationToken token);

        protected virtual Task OnEventAdded(EventRecord @event, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        void ThrowIfDuplicateEventId(Id eventId)
        {
            if (events!.Any(e => e.Id == eventId))
                throw new DuplicateEventException(eventId, streamId);
        }

        void ThrowIfThereIsDuplicates()
        {
            var duplicateRevisions =
             events.GroupBy(e => e.Revision)
             .Where(g => g.Count() > 1)
             .Select(g => g.Key)
             .ToArray();

            if (duplicateRevisions.Any())
                throw new DuplicateRevisionException(duplicateRevisions[0], streamId);

            var duplicateId =
                events.GroupBy(e => e.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .FirstOrDefault();

            if (duplicateId != Id.None)
                throw new DuplicateEventException(duplicateId, streamId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
