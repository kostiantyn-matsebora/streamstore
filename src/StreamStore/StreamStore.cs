using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.Stream;



namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;
        readonly int pageSize = 10;
        readonly StreamEventEnumeratorFactory enumeratorFactory;
        public StreamStore(StreamStoreConfiguration configuration, IStreamDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            converter = new EventConverter(serializer);
            enumeratorFactory = new StreamEventEnumeratorFactory(configuration, database, converter);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            await database.DeleteAsync(streamId, cancellationToken);
        }

        public  async Task<IAsyncEnumerable<StreamEvent>> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "startFrom must be greater than or equal to 1.");

            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            if (metadata == null) throw new StreamNotFoundException(streamId);

            if (metadata.MaxRevision < startFrom)
                throw new InvalidStartFromException(streamId, startFrom, metadata.MaxRevision);

            var parameters = new StreamReadingParameters(streamId, startFrom, pageSize);

            return new StreamEventEnumerable(parameters, enumeratorFactory);
        }

        public async Task<IStreamWriter> BeginWriteAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            if (expectedRevision < Revision.Zero)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision), "expectedRevision must be greater than or equal to 0.");

            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            
            if (metadata == null) metadata = new EventMetadataRecordCollection();


            if (expectedRevision != metadata.MaxRevision)
                throw new OptimisticConcurrencyException(expectedRevision, metadata.MaxRevision, streamId);

            var uow = await database.BeginAppendAsync(streamId, expectedRevision);

            if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");

            return new StreamWriter(uow, converter);
        }
    }
}
