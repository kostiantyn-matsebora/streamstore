using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    class Stream: IStream
    {

        List<Id>? eventIds;
        int revision;
        string? streamId;

        IEventUnitOfWork? uow;
        readonly IEventDatabase database;
        readonly IEventSerializer serializer;

        public Stream(IEventDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            this.database = database;
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            this.serializer = serializer;
        }


        public async Task OpenAsync(string streamId, int expectedRevision, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(streamId))
                throw new ArgumentNullException(nameof(streamId));
            if (expectedRevision <= 0)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision), "Expected revision must be greater or equal 0");

            this.streamId = streamId;
            eventIds = new List<Id>();
            revision = 0;
            uow = null;

            var stream = await database.FindMetadataAsync(streamId, cancellationToken);

            if (stream != null)
            {
                if (expectedRevision != stream.Revision)
                   throw new OptimisticConcurrencyException(expectedRevision, stream.Revision, streamId);

                eventIds!.AddRange(stream.Events.Select(e => e.Id));
                revision = stream.Revision;
            }

           uow = database.CreateUnitOfWork(streamId, expectedRevision);
        }

        public IStream Add(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(streamId))
                throw new InvalidOperationException("Stream is not open.");

            if (eventIds!.Contains(eventId))
                throw new DuplicateEventException(eventId, streamId!);

            eventIds!.Add(eventId);
            revision++;

            uow!.Add(eventId, revision, timestamp, serializer.Serialize(@event));
            return this;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await uow!.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            uow?.Dispose();
            uow = null;
        }
    }
}
