using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;



namespace StreamStore
{
    internal class StreamEventProducer
    {
        readonly EventConverter converter;
        readonly IStreamDatabase database;

        public StreamEventProducer(IStreamDatabase database, EventConverter converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IAsyncEnumerable<EventEntity> StartProducing(StreamReadingParameters parameters, CancellationToken token)
        {
            var channel = CreateChannel(parameters.PageSize);

            _ = ProduceAsync(parameters, channel, token);

            return channel.Reader.ReadAllAsync(token);
        }

        Channel<EventEntity> CreateChannel(int pageSize)
        {
            return Channel.CreateBounded<EventEntity>(
                new BoundedChannelOptions(pageSize)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = true,
                    SingleWriter = true
                });
        }

        async Task ProduceAsync(StreamReadingParameters parameters, ChannelWriter<EventEntity> writer, CancellationToken token)
        {
            int cursor = parameters.StartFrom;
            EventRecord[] records;

            do
            {
                if (token.IsCancellationRequested) break;

                records = await ReadPageAsync(parameters, cursor, token);

                if (records.Length == 0) break;

                cursor = await WritePageAsync(records, writer, cursor, token);

            } while (records.Length > 0);

            writer.Complete();
        }

        async Task<int> WritePageAsync(EventRecord[] records, ChannelWriter<EventEntity> writer, int cursor, CancellationToken token)
        {
            foreach (var record in records)
            {
                if (token.IsCancellationRequested) return cursor;

                await writer.WriteAsync(converter.ConvertToEntity(record));

                cursor = record.Revision;
            }

            return cursor;
        }

        async Task<EventRecord[]> ReadPageAsync(StreamReadingParameters parameters, Revision cursor, CancellationToken token)
        {
            return await database.ReadAsync(parameters.StreamId, cursor, parameters.PageSize, token);
        }
    }
}

