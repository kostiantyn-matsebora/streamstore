using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Serialization;


namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;

        public StreamStore(IStreamDatabase database) : this(database, new EventSerializer())
        {
        }

        public StreamStore(IStreamDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            converter = new EventConverter(serializer);
        }

        public Task<IStream> OpenStreamAsync(Id streamId, CancellationToken cancellationToken = default)
        {            
            return OpenStreamAsync(streamId, 0, cancellationToken);
        }

        public async Task<IStream> OpenStreamAsync(Id streamId, int expectedRevision, CancellationToken cancellationToken = default)
        {
            var stream = new Stream(database, converter);
            await stream.OpenAsync(streamId, expectedRevision, cancellationToken);
            return stream;
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId), "streamId is required.");

            await database.DeleteAsync(streamId, cancellationToken);
        }

        public async Task<StreamEntity> GetAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId), "streamId is required.");

            var streamRecord =
                await database.FindAsync(streamId, cancellationToken);

            if (streamRecord == null)
                throw new StreamNotFoundException(streamId);

            return converter.ConvertToEntity(streamRecord);
        }
    }
}
