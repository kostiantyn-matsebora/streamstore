using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading;
using System;
using System.Linq;

namespace StreamStore.InMemory
{
    class InMemoryEventUnitOfWork : IEventUnitOfWork
    {
        readonly StreamRecord stream;
        readonly int expectedStreamVersion;
        readonly InMemoryDatabase table;
        readonly List<EventRecord> events = new List<EventRecord>();

        public InMemoryEventUnitOfWork(string streamId, int expectedStreamVersion, InMemoryDatabase table, StreamRecord? existing)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            this.expectedStreamVersion = expectedStreamVersion;
            this.table = table;

            if (existing != null)
            {
                stream = existing;
                events = new List<EventRecord>(existing.Events);
            }
            else
            {
                stream = new StreamRecord(streamId);
                events = new List<EventRecord>();
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            var record = new StreamRecord(stream.Id, events);
            table.store.AddOrUpdate(stream.Id, record, (key, oldValue) =>
            {
                if (oldValue.Revision != expectedStreamVersion)
                    throw new OptimisticConcurrencyException(expectedStreamVersion, oldValue.Revision, stream.Id);
                ThrowIfThereIsDuplicates();
                return record;
            });

            return Task.CompletedTask;
        }

        public IEventUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data)
        {
            ThrowIfDuplicateEventId(eventId);
            ThrowIfRevisionAlreadyExists(revision);

            events.Add(
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
            if (events.Exists(e => e.Revision == revision))
                throw new OptimisticConcurrencyException(revision, stream.Id);
        }

        void ThrowIfDuplicateEventId(Id eventId)
        {
            if (events.Exists(e => e.Id == eventId))
                throw new DuplicateEventException(eventId, stream.Id);
        }

        void ThrowIfThereIsDuplicates()
        {
            var duplicateRevision =
             events.GroupBy(e => e.Revision)
             .Where(g => g.Count() > 1)
             .Select(g => g.Key)
             .FirstOrDefault();

            if (duplicateRevision != default)
                throw new OptimisticConcurrencyException(duplicateRevision, stream.Id);

            var duplicateId = 
                events.GroupBy(e => e.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .FirstOrDefault();

            if (duplicateId != Id.None)
                throw new DuplicateEventException(duplicateId, stream.Id);
        }


        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
