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
        readonly IStreamStorage storage;
        readonly EventConverter converter;
        readonly StreamStoreConfiguration configuration;
        readonly StreamEventEnumeratorFactory enumeratorFactory;
        public StreamStore(StreamStoreConfiguration configuration, IStreamStorage storage, IEventSerializer serializer)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.storage = storage;
            converter = new EventConverter(serializer);
            enumeratorFactory = new StreamEventEnumeratorFactory(configuration, storage, converter);
            this.configuration = configuration;
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            await storage.DeleteAsync(streamId, cancellationToken);
        }

        public async Task<IAsyncEnumerable<StreamEvent>> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "startFrom must be greater than or equal to 1.");

            var revision = await storage.GetActualRevision(streamId, cancellationToken);
            if (revision == null) throw new StreamNotFoundException(streamId);

            if (revision.Value < startFrom)
                throw new InvalidStartFromException(streamId, startFrom, revision.Value);

            var parameters = new StreamReadingParameters(streamId, startFrom, configuration.ReadingPageSize);

            return new StreamEventEnumerable(parameters, enumeratorFactory);
        }

        public async Task<IStreamUnitOfWork> BeginWriteAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            var revision = await storage.GetActualRevision(streamId, cancellationToken);

            if (revision == null) revision = Revision.Zero;


            if (expectedRevision != revision.Value)
                throw new OptimisticConcurrencyException(expectedRevision, revision.Value, streamId);

            var uow = await storage.BeginAppendAsync(streamId, expectedRevision);

            if (uow == null)
                throw new InvalidOperationException("Failed to open stream, either stream does not exist or revision is incorrect.");

            return new StreamUnitOfWork(uow, converter);
        }
    }
}
