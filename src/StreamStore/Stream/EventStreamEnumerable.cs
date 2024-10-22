using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.Stream
{
    class EventStreamEnumerator : IAsyncEnumerator<EventEntity>
    {
        readonly IStreamDatabase database;
        private readonly EventConverter converter;
        readonly Id streamId;
        readonly int pageSize;
        Revision nextRevision;

        readonly Queue<EventRecord> queue = new Queue<EventRecord>();

        public EventStreamEnumerator(StreamReadingParameters parameters, IStreamDatabase database, EventConverter converter)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            if (parameters == null) throw new ArgumentNullException(nameof(converter));
            streamId = parameters.StreamId;
            pageSize = parameters.PageSize;
            nextRevision = parameters.StartFrom;
            Current = null!;
        }

        public EventEntity Current { get; private set; }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (MoveNext()) return true;

            var records = await ReadNextPage();

            if (!records.Any())
            {
                Current = null!;
                return false;
            }

            foreach (var r in records) queue.Enqueue(r);

            return MoveNext();
        }

        async Task<EventRecordCollection> ReadNextPage()
        {
            return new EventRecordCollection(await database.ReadAsync(streamId, nextRevision, pageSize));
        }

        bool MoveNext()
        {
            if (queue.TryDequeue(out EventRecord record))
            {
                Current = converter.ConvertToEntity(record);
                nextRevision = Current.Revision + 1;
            }

            Current = null!;
            return false;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }

    class EventStreamEnumerable: IAsyncEnumerable<EventEntity>
    {
        readonly IStreamDatabase database;
        readonly StreamReadingParameters parameters;
        readonly EventConverter converter;

        public EventStreamEnumerable(StreamReadingParameters parameters, IStreamDatabase database, EventConverter converter)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new EventStreamEnumerator(parameters, database, converter);
        }
    }
}
