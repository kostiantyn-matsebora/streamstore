using StreamStore.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;


namespace StreamStore
{
    public abstract class StreamUnitOfWorkBase : IStreamUnitOfWork
    {
        protected readonly Id streamId;
        protected readonly Revision expectedRevision;
        readonly EventRecordCollection events = new EventRecordCollection();
        readonly EventRecordCollection uncommited = new EventRecordCollection();

        Revision revision;

        protected StreamUnitOfWorkBase(Id streamId, Revision expectedRevision, StreamRecord? existing)
        {
            streamId.ThrowIfHasNoValue();

            this.streamId = streamId;

            if (existing != null && !existing.IsEmpty)
            {
                events.AddRange(existing.Events);
            }

            this.expectedRevision = expectedRevision;
            revision = expectedRevision;
            
        }

        public async Task<Revision> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ThrowIfThereIsDuplicates();

            await SaveChangesAsync(uncommited, cancellationToken);
            return events.MaxRevision;
        }

        public async Task<IStreamUnitOfWork> AddAsync(Id eventId, DateTime timestamp, string data, CancellationToken token = default)
        {
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

        protected abstract Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token);

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

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal() { }
    }

}
