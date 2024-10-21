using System;
using System.Threading;
using System.Threading.Tasks;



namespace StreamStore
{
    public sealed class StreamStore : IStreamStore
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;

        public StreamStore(IStreamDatabase database, IEventSerializer serializer)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.database = database;
            converter = new EventConverter(serializer);
        }

        public async Task<IStream> OpenStreamAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            var metadata = await database.FindMetadataAsync(streamId, cancellationToken);
            if (metadata == null)
              metadata = new StreamMetadataRecord();

            return new Stream(streamId, metadata, database, converter);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId), "streamId is required.");

            await database.DeleteAsync(streamId, cancellationToken);
        }
    }
}
