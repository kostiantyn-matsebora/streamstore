using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.API;
using StreamStore.Domain;
using StreamStore.Serialization;


namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IEventDatabase database;
        readonly IEventSerializer serializer;
        readonly Converter converter = new Converter(new EventSerializer());

        public StreamStore(IEventDatabase database) : this(database, new EventSerializer())
        {
        }

        public StreamStore(IEventDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            this.serializer = serializer;
        }

        public async Task<IStream> OpenStreamAsync(string streamId, int expectedRevision, CancellationToken cancellationToken = default)
        {
            var stream = new Stream(database, serializer);
            await stream.OpenAsync(streamId, expectedRevision, cancellationToken);
            return stream;
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException("streamId is required.", nameof(streamId));

            await database.DeleteAsync(streamId, cancellationToken);
        }

        public async Task<StreamEntity> GetAsync(string streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException("streamId is required.", nameof(streamId));

            var streamRecord =
                await database.FindAsync(streamId, cancellationToken);

            if (streamRecord == null)
                throw new StreamNotFoundException(streamId);

            return converter.ToEntity(streamRecord);
        }
    }
}
