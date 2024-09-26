using System;

using System.Threading;
using System.Threading.Tasks;
using StreamDB.Operations;


namespace StreamDB
{
    public sealed class StreamDB: IStreamDB
    {
        readonly IEventStore store;
        readonly IEventSerializer serializer;

        public StreamDB(IEventStore store) : this(store, new EventSerializer())
        {
            this.store = store;
            this.serializer = new EventSerializer();
        }

        public StreamDB(IEventStore store, IEventSerializer serializer)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            this.store = store;
            this.serializer = serializer;
        }

        public async Task AppendAsync(string streamId, IStreamItem[] uncommited, int expectedRevision, CancellationToken cancellationToken = default)
        {
           await new AppendToStreamOperation(store, serializer)
                .AddStreamId(streamId)
                .AddTransientEvents(uncommited)
                .AddExpectedRevision(expectedRevision)
                .ExecuteAsync(cancellationToken);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken = default)
        {
           await new DeleteOperation(store)
                    .AddStreamId(streamId)
                    .ExecuteAsync(cancellationToken);
        }

        public async Task<IStream> GetAsync(string streamId, CancellationToken cancellationToken = default)
        {
            return 
                await new GetStreamOperation(store, serializer)
                 .AddStreamId(streamId)
                 .ExecuteAsync(cancellationToken);
        }
    }
}
