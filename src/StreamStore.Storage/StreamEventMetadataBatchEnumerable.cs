using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Extensions;


namespace StreamStore.Storage
{
    public delegate Task<T[]> ReadBatch<T>(Revision startFrom, int count) where T: IStreamEventMetadata;

    public sealed class StreamEventMetadataBatchEnumerable<T> : IAsyncEnumerable<T[]> where T: IStreamEventMetadata
    {
        readonly StreamEventReadingParameters parameters;
        readonly ReadBatch<T> reader;

        public StreamEventMetadataBatchEnumerable(StreamEventReadingParameters parameters, ReadBatch<T> reader)
        {
            this.parameters = parameters.ThrowIfNull(nameof(parameters));
            this.reader = reader.ThrowIfNull(nameof(reader));
        }

        public IAsyncEnumerator<T[]> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new StreamEventMetadataBatchEnumerator<T>(parameters, reader);
        }
    }

    internal class StreamEventMetadataBatchEnumerator<T> : IAsyncEnumerator<T[]> where T : IStreamEventMetadata
    {
        readonly StreamEventReadingParameters parameters;
        readonly ReadBatch<T> reader;
        Revision startFrom;
        int count = 0;

        public StreamEventMetadataBatchEnumerator(StreamEventReadingParameters parameters, ReadBatch<T> reader)
        {
            this.parameters = parameters.ThrowIfNull(nameof(parameters));
            this.reader = reader.ThrowIfNull(nameof(reader));
            Reset();
        }

        public T[] Current { get; private set; } = Array.Empty<T>();


        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (count >= parameters.Count) return false;

            var leftover = parameters.Count - count;
            var batchSize = leftover >= parameters.BatchSize ? parameters.BatchSize : leftover;

            Current = await reader(startFrom, batchSize);
            if (Current.Length == 0) return false;

            count = count + Current.Length;

            startFrom = ((Revision)Current.Last().Revision).Next();
            return true;
        }

        public void Reset()
        {
            startFrom = parameters.StartFrom;
        }
    }
}
