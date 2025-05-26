using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.Stream;



namespace StreamStore
{
    class StreamStore : IStreamStore
    {
        readonly IStreamStorage storage;
        readonly StreamStoreConfiguration configuration;
        readonly StreamEventEnumeratorFactory enumeratorFactory;
        readonly IStreamUnitOfWorkFactory uowFactory;
        public StreamStore(StreamEventEnumeratorFactory enumeratorFactory, StreamStoreConfiguration configuration, IStreamStorage storage, IStreamUnitOfWorkFactory uowFactory)
        {
            this.storage = storage.ThrowIfNull(nameof(storage));
            this.enumeratorFactory = enumeratorFactory.ThrowIfNull(nameof(enumeratorFactory)); 
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
            this.uowFactory = uowFactory.ThrowIfNull(nameof(uowFactory));
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            await storage.DeleteAsync(streamId, cancellationToken);
        }

        public async Task<IAsyncEnumerable<IStreamEventEnvelope>> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "startFrom must be greater than or equal to 1.");

            var metadata = await storage.GetMetadataAsync(streamId, cancellationToken);
            if (metadata == null) throw new StreamNotFoundException(streamId);

            if (metadata.Revision < startFrom)
                throw new InvalidStartFromException(streamId, startFrom, metadata.Revision);

            var parameters = new StreamReadingParameters(streamId, startFrom, configuration.ReadingPageSize);

            return new StreamEventEnumerable(parameters, enumeratorFactory);
        }

        public async Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            var metadata = await storage.GetMetadataAsync(streamId, cancellationToken);

            Revision revision = Revision.Zero;

            if (metadata != null) revision = metadata.Revision;

            if (expectedRevision != revision)
                throw new InvalidStreamRevisionException(streamId, expectedRevision, revision);

            return uowFactory.Create(streamId, expectedRevision);
        }

        public async Task<IStreamMetadata> GetMetadataAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            var metadata = await storage.GetMetadataAsync(streamId, cancellationToken);
            if (metadata == null)
            {
                throw new StreamNotFoundException(streamId);
            }

            return metadata;
        }
    }
}
