using System;
using StreamStore.Exceptions;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Channels;



namespace StreamStore
{
    partial class Stream : IStream
    {
        readonly EventConverter converter;
        readonly StreamMetadataRecord metadata;
        readonly IStreamDatabase database;
        readonly Id streamId;
        Revision currentRevision;

        public Stream(Id streamId, StreamMetadataRecord metadata, IStreamDatabase database, EventConverter converter)
        {
            streamId.ThrowIfHasNoValue();
            this.streamId = streamId;
            this.metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            eventTracking!.AddRange(metadata.EventIds);

            this.currentRevision = metadata.Revision;
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public async Task<IWriteOnlyStream> BeginWriteAsync(Revision expectedRevision, CancellationToken cancellationToken = default)
        {

            if (expectedRevision != currentRevision)
                throw new OptimisticConcurrencyException(expectedRevision, metadata.Revision, streamId);

            uow = await database.BeginAppendAsync(streamId, expectedRevision);

            if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");

            isOpened = true;

            return this;
        }

        public IReadOnlyStream BeginReadAsync(CancellationToken token = default)
        {
            var channel = CreateChannel(pageSize);
            StartProducing(channel.Writer, token);
            return new ReadOnlyStream(channel.Reader);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow?.Dispose();
                uow = null;
            }
        }

        public async Task<IWriteOnlyStream> WriteAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default)
        {
            currentRevision = 
                await  BeginWriteAsync(currentRevision, cancellationToken)
                        .AddAsync(eventId, timestamp, @event, cancellationToken)
                       .SaveChangesAsync(cancellationToken);
            return this;
        }

        public async Task<IWriteOnlyStream> WriteAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            await BeginWriteAsync(currentRevision, cancellationToken);

            foreach (var @event in events)
            {
                await AddAsync(@event.Id, @event.Timestamp, @event.EventObject, cancellationToken);
            }

            currentRevision = await SaveChangesAsync(cancellationToken);

            return this;
        }

        public IReadOnlyStream BeginReadAsync(Revision startFrom, CancellationToken cancellationToken = default)
        {
            if (currentRevision < startFrom)
                throw new InvalidOperationException("Cannot start reading from a revision greater than the current revision.");

            var channel = CreateChannel(pageSize);
            StartProducing(channel.Writer, cancellationToken);
            StartReading(channel.Reader);
            return new ReadOnlyStream(channel.Reader);
        }

        private static IAsyncEnumerable<EventEntity> StartReading(ChannelReader<EventEntity> reader)
        {
            return reader.ReadAllAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
