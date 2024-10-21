using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    internal class StreamEventProducerFactory
    {
        readonly IStreamDatabase database;
        readonly EventConverter converter;

        public StreamEventProducerFactory(IStreamDatabase database, EventConverter converter)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public Task StartProducing(StreamReadingParameters parameters, StreamEventQueue queue, CancellationToken token)
        {
            return  Task.Run(() =>
            {
                var cursor = parameters.StartFrom;
                EventRecord[] records = ReadPage(parameters, cursor, token);

                while (records != null && records.Length > 0)
                {
                    foreach (var record in records)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                        queue.Add(converter.ConvertToEntity(record));
                        cursor = record.Revision;
                    }
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    records = ReadPage(parameters, cursor, token);
                }
                queue.Complete();
            }, token);
        }

        private EventRecord[] ReadPage(StreamReadingParameters parameters, Revision cursor, CancellationToken token)
        {
            return database
                    .ReadAsync(parameters.StreamId, cursor, parameters.PageSize, token)
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
