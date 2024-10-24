using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace StreamStore.Stream
{
    internal sealed class ProduceConsumeStreamEventEnumerator : IAsyncEnumerator<StreamEvent>
    {
        readonly IStreamReader reader;
        readonly EventConverter converter;
        readonly StreamReadingParameters parameters;
        readonly CancellationToken token;
        readonly Channel<StreamEvent> channel;
        readonly Task producer;

        public ProduceConsumeStreamEventEnumerator(
            StreamReadingParameters parameters,
            IStreamReader reader, 
            EventConverter converter, 
            CancellationToken token)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.token = token;
            this.channel = CreateChannel(parameters.PageSize);
            this.producer = ProduceAsync();
            Current = null!;
        }


 
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var entity = await channel.Reader.ReadAsync();
            if (entity == null)
            {
                Current = null!;
                return false;
            }

            Current = entity;
            return true;
        }
        public StreamEvent Current { get; private set; }

        Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                channel.Writer.Complete();
                producer.Dispose();
            }

            return Task.CompletedTask;
        }

        Channel<StreamEvent> CreateChannel(int pageSize)
        {
            return Channel.CreateBounded<StreamEvent>(
                new BoundedChannelOptions(pageSize)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = true,
                    SingleWriter = true
                });
        }

        async Task ProduceAsync()
        {
            int cursor = parameters.StartFrom;
            EventRecord[] records;

            do
            {
                if (token.IsCancellationRequested) break;

                records = await reader.ReadAsync(parameters.StreamId, cursor, parameters.PageSize, token);

                if (records.Length == 0) break;

                cursor = await WritePageAsync(records, cursor);

            } while (records.Length > 0);

            channel.Writer.Complete();
        }

        async Task<int> WritePageAsync(EventRecord[] records, int cursor)
        {
            foreach (var record in records)
            {
                if (token.IsCancellationRequested) return cursor;

                await channel.Writer.WriteAsync(converter.ConvertToEvent(record), token);

                cursor = record.Revision;
            }

            return cursor;
        }
    }
}
