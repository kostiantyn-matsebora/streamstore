using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.Stream
{
    class DefaultStreamEventEnumerator : IAsyncEnumerator<IStreamEventEnvelope>
    {
        readonly IStreamReader reader;
        private readonly IEventConverter converter;
        private readonly CancellationToken token;
        readonly Id streamId;
        readonly int pageSize;
        Revision nextRevision;

        readonly Queue<IStreamEventRecord> queue = new Queue<IStreamEventRecord>();

        public DefaultStreamEventEnumerator(StreamReadingParameters parameters, IStreamReader reader, IEventConverter converter, CancellationToken token)
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

        public IStreamEventEnvelope Current { get; private set; }

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

        async Task<IStreamEventRecord[]> ReadNextPage()
        {
            return await reader.ReadAsync(streamId, nextRevision, pageSize);
        }

        bool MoveNext()
        {
            if (queue.TryDequeue(out IStreamEventRecord record))
            {
                Current = converter.ConvertToEnvelope(record);
                nextRevision = Current.Revision + 1;
                return true;
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
