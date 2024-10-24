using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;



namespace StreamStore
{
    internal class DefaultStreamEventProducer
    {
        readonly EventConverter converter;
        readonly IStreamDatabase database;

        public DefaultStreamEventProducer(IStreamDatabase database, EventConverter converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IAsyncEnumerable<StreamEvent> StartProducing(StreamReadingParameters parameters, CancellationToken token)
        {
            var channel = CreateChannel(parameters.PageSize);

            _ = ProduceAsync(parameters, channel, token);

            return channel.Reader.ReadAllAsync(token);
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

        async Task ProduceAsync(StreamReadingParameters parameters, ChannelWriter<StreamEvent> writer, CancellationToken token)
        {
            int cursor = parameters.StartFrom;
            EventRecord[] records;

            do
            {
                if (token.IsCancellationRequested) break;

                records = await database.ReadAsync(parameters.StreamId, cursor, parameters.PageSize, token);

                if (records.Length == 0) break;

                cursor = await WritePageAsync(records, writer, cursor, token);

            } while (records.Length > 0);

            writer.Complete();
        }

        async Task<int> WritePageAsync(EventRecord[] records, ChannelWriter<StreamEvent> writer, int cursor, CancellationToken token)
        {
            foreach (var record in records)
            {
                if (token.IsCancellationRequested) return cursor;

                await writer.WriteAsync(converter.ConvertToEvent(record), token);

                cursor = record.Revision;
            }

            return cursor;
        }
    }
}

