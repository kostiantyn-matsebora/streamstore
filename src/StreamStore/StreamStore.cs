using System;
using System.Threading;
using System.Threading.Tasks;



namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;
        readonly StreamEventProducerFactory producerFactory;
        public StreamStore(IStreamDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            converter = new EventConverter(serializer);
            producerFactory = new StreamEventProducerFactory(database, converter);
        }

        public async Task<IStream> OpenAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            if (metadata == null) metadata = new StreamMetadataRecord();

            var ctx = new StreamContext(streamId, metadata);

            return new Stream(ctx, database, converter, producerFactory);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId), "streamId is required.");

            await database.DeleteAsync(streamId, cancellationToken);
        }
    }
}
