using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;


namespace StreamStore
{
    sealed class Stream: IStream
    {

        readonly List<Id>? eventTracking = new List<Id>();
        string? streamId;
        bool isOpened;

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


        public async Task OpenAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue();

            this.streamId = streamId;

            var stream = await database.FindMetadataAsync(streamId, cancellationToken);

            if (stream != null)
            {
                if (expectedRevision != stream.Revision)
                   throw new OptimisticConcurrencyException(expectedRevision, stream.Revision, streamId);

                eventTracking!.AddRange(stream.EventIds);
            }

           uow = await database.BeginAppendAsync(streamId, expectedRevision);

           if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");
           isOpened = true;
        }

        public async Task<IStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
        {
            if (!isOpened)
                throw new InvalidOperationException("Stream is not open.");

            if (eventId == Id.None)
                throw new ArgumentNullException(nameof(eventId));

            if (eventTracking!.Contains(eventId))
                throw new DuplicateEventException(eventId, streamId!);

            eventTracking!.Add(eventId);

            await uow!.AddAsync(eventId, timestamp, converter.ConvertToByteArray(@event));
            return this;
        }

        public async Task<Revision> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await uow!.SaveChangesAsync(cancellationToken);
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
