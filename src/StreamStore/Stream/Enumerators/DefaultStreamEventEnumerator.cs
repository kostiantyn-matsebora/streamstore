using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.Stream
{
    class DefaultStreamEventEnumerator : IAsyncEnumerator<StreamEvent>
    {
        readonly IStreamReader reader;
        private readonly EventConverter converter;
        private readonly CancellationToken token;
        readonly Id streamId;
        readonly int pageSize;
        Revision nextRevision;

        readonly Queue<EventRecord> queue = new Queue<EventRecord>();

        public DefaultStreamEventEnumerator(StreamReadingParameters parameters, IStreamReader reader, EventConverter converter, CancellationToken token)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            this.token = token;
            if (parameters == null) throw new ArgumentNullException(nameof(converter));
            streamId = parameters.StreamId;
            pageSize = parameters.PageSize;
            nextRevision = parameters.StartFrom;
            Current = null!;
        }

        public StreamEvent Current { get; private set; }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (MoveNext()) return true;

            if (token.IsCancellationRequested) return false;

            var records = await ReadNextPage();

            if (!records.Any())
            {
                Current = null!;
                return false;
            }

            foreach (var r in records)
            {
                if (token.IsCancellationRequested) return false;
                queue.Enqueue(r);
            }

            if (token.IsCancellationRequested) return false;
            return MoveNext();
        }

        async Task<EventRecordCollection> ReadNextPage()
        {
            return new EventRecordCollection(await reader.ReadAsync(streamId, nextRevision, pageSize));
        }

        bool MoveNext()
        {
            if (queue.TryDequeue(out EventRecord record))
            {
                Current = converter.ConvertToEvent(record);
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
}
