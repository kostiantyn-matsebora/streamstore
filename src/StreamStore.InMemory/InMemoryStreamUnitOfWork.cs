using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading;
using System;
using System.Linq;



namespace StreamStore.InMemory
{
    sealed class InMemoryStreamUnitOfWork : IStreamUnitOfWork
    {
        
        int expectedStreamVersion;
        InMemoryStreamDatabase database;
        string streamId;
        List<EventRecord>? events;

        public InMemoryStreamUnitOfWork(string streamId, int expectedStreamVersion, InMemoryStreamDatabase database, StreamRecord? existing)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            if (string.IsNullOrEmpty(streamId))
                throw new ArgumentNullException(nameof(streamId));

            this.streamId = streamId;
            this.expectedStreamVersion = expectedStreamVersion;
            this.database = database;

            events = existing?.Events.Any() == true
                ? new List<EventRecord>(existing.Events)
                : new List<EventRecord>();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            var record = new StreamRecord(streamId, events!);
            database.store.AddOrUpdate(streamId, record, (key, oldValue) =>
            {
                if (oldValue.Revision != expectedStreamVersion)
                    throw new OptimisticConcurrencyException(expectedStreamVersion, oldValue.Revision, key);
                ThrowIfThereIsDuplicates();
                return record;
            });

            return Task.CompletedTask;
        }

        public IStreamUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data)
        {
            ThrowIfDuplicateEventId(eventId);
            ThrowIfRevisionAlreadyExists(revision);

            events!.Add(
                new EventRecord
                {
                    Id = eventId,
                    Revision = revision,
                    Timestamp = timestamp,
                    Data = data
                }
             );
            return this;
        }


        void ThrowIfRevisionAlreadyExists(int revision)
        {
            if (events!.Exists(e => e.Revision == revision))
                throw new OptimisticConcurrencyException(revision, streamId);
        }

        void ThrowIfDuplicateEventId(Id eventId)
        {
            if (events!.Exists(e => e.Id == eventId))
                throw new DuplicateEventException(eventId, streamId);
        }

        void ThrowIfThereIsDuplicates()
        {
            var duplicateRevision =
             events.GroupBy(e => e.Revision)
             .Where(g => g.Count() > 1)
             .Select(g => g.Key)
             .FirstOrDefault();

            if (duplicateRevision != default)
                throw new OptimisticConcurrencyException(duplicateRevision, streamId);

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
                expectedStreamVersion = 0;
                database = null!;
                streamId = null!;
                events = null!;
            }
        }
    }
}
