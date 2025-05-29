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

        public async Task<IAsyncEnumerable<IStreamEventEnvelope>> BeginReadAsync(Id streamId, Revision fromRevision, CancellationToken cancellationToken = default)
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));

            if (fromRevision < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(fromRevision), "fromRevision must be greater than or equal to 1.");

            var metadata = await storage.GetMetadataAsync(streamId, cancellationToken);
            if (metadata == null) throw new StreamNotFoundException(streamId);

            if (metadata.Revision < fromRevision)
                throw new InvalidFromRevisionException(streamId, fromRevision, metadata.Revision);

            var parameters = new StreamReadingParameters(streamId, fromRevision, configuration.ReadingPageSize);

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
