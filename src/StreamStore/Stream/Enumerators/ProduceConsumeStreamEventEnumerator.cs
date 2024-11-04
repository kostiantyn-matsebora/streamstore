using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using StreamStore.Exceptions;


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
            StreamEvent streamEvent = null!;
            try
            {
                streamEvent = await channel.Reader.ReadAsync();
            }
            catch (ChannelClosedException)
            {
                // Channel is closed when there is nothing more to read.
            }

            if (streamEvent == null)
            {
                Current = null!;
                return false;
            }

            Current = streamEvent;
            return true;
        }
        public StreamEvent Current { get; private set; }

        Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {             
                if (producer.Status == TaskStatus.Running)
                {
                    channel.Writer.Complete();
                }
                producer.Wait();
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
            EventRecordCollection records;

            do
            {
                if (token.IsCancellationRequested) break;
                try
                {
                    records = await reader.ReadAsync(parameters.StreamId, cursor, parameters.PageSize, token);
                }
                catch (StreamNotFoundException)
                {
                    channel.Writer.Complete();
                    throw;
                }

                if (records.Count() == 0) break;

                cursor = await WritePageAsync(records, cursor);

            } while (records.Count() > 0);

            channel.Writer.Complete();
        }

        async Task<int> WritePageAsync(EventRecordCollection records, int cursor)
        {
            foreach (var record in records)
            {
                if (token.IsCancellationRequested) return cursor;

                await channel.Writer.WriteAsync(converter.ConvertToEvent(record), token);

                cursor = record.Revision + 1;
            }

            return cursor;
        }
    }
}
