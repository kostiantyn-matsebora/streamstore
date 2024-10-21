using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;



namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;
        readonly StreamEventProducer producer;
        readonly int pageSize = 10;

        public StreamStore(IStreamDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            converter = new EventConverter(serializer);
            producer = new StreamEventProducer(database, converter);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId), "streamId is required.");

            await database.DeleteAsync(streamId, cancellationToken);
        }

        public  async Task<IEventStreamReader> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default)
        {
            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            if (metadata == null) throw new StreamNotFoundException(streamId);

            if (metadata.Revision < startFrom)
                throw new InvalidOperationException("Cannot start reading from a revision greater than the current revision.");

            var parameters = new StreamReadingParameters(streamId, startFrom, pageSize);

            return new EventStreamReader(parameters, producer);
        }

        public async Task<IEventStreamWriter> BeginWriteAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            if (metadata == null) metadata = new StreamMetadataRecord();


            if (expectedRevision != metadata.Revision)
                throw new OptimisticConcurrencyException(expectedRevision, metadata.Revision, streamId);

            var uow = await database.BeginAppendAsync(streamId, expectedRevision);

            if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");

            return new EventStreamWriter(uow, converter);

        }
    }
}
