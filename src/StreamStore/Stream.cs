using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    sealed class Stream: IStream
    {

        List<Id>? eventIdentifiers = new List<Id>();
        int revision;
        string? streamId;

        IStreamUnitOfWork? uow;
        readonly EventConverter converter;
        readonly IStreamDatabase database;


        public Stream(IStreamDatabase database, EventConverter converter)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            this.database = database;
            
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            this.converter = converter;
        }


        public async Task OpenAsync(Id streamId, int expectedRevision, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(streamId))
                throw new ArgumentNullException(nameof(streamId));
            if (expectedRevision < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision), "Expected revision must be greater or equal 0");

            this.streamId = streamId;

            revision = 0;
            uow = null;

            var stream = await database.FindMetadataAsync(streamId, cancellationToken);

            if (stream != null)
            {
                if (expectedRevision != stream.Revision)
                   throw new OptimisticConcurrencyException(expectedRevision, stream.Revision, streamId);

                eventIdentifiers!.AddRange(stream.EventIds);
                revision = stream.Revision;
            }

           uow = database.BeginAppend(streamId, expectedRevision);

           if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");
        }

        public IStream Add(Id eventId, DateTime timestamp, object @event)
        {
            if (string.IsNullOrEmpty(streamId))
                throw new InvalidOperationException("Stream is not open.");

            if (eventId == Id.None)
                throw new ArgumentNullException(nameof(eventId));

            if (eventIdentifiers!.Contains(eventId))
                throw new DuplicateEventException(eventId, streamId!);

            eventIdentifiers!.Add(eventId);
            revision++;

            uow!.Add(eventId, revision, timestamp, converter.ConvertToString(@event));
            return this;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await uow!.SaveChangesAsync(cancellationToken);
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
                uow?.Dispose();
                uow = null;
            }
        }
    }
}
