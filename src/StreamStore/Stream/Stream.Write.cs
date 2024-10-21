using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;


namespace StreamStore
{
    partial class Stream: IWriteOnlyStream
    {

        readonly List<Id>? eventTracking = new List<Id>();
        bool isOpened;
        IStreamUnitOfWork? uow;

        public async Task<IWriteOnlyStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
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
    }
}
