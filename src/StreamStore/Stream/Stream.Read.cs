

using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore
{
    partial class  Stream
    {
        int pageSize;
        Revision startFrom;

        void StartProducing(ChannelWriter<EventEntity> channel, CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                var cursor = startFrom;

                var records = ReadPage(cursor, token);

                while (records != null && records.Length > 0)
                {
                    WritePageToChannel(channel, ref cursor, records, token);

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    records = ReadPage(cursor, token);
                }
            }, token);
        }

        void WritePageToChannel(ChannelWriter<EventEntity> writer, ref Revision cursor, EventRecord[] records, CancellationToken token)
        {
            foreach (var record in records)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                writer.TryWrite(converter.ConvertToEntity(record));
                cursor = ++record.Revision;
            }
        }

        EventRecord[] ReadPage(int cursor, CancellationToken token)
        {
            return
                database
                .ReadAsync(streamId, cursor, pageSize, token)
                .GetAwaiter()
                .GetResult();
        }

        Channel<EventEntity> CreateChannel(int pageSize)
        {
            return Channel.CreateBounded<EventEntity>(new BoundedChannelOptions(pageSize)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = true
            });
        }
    }
}
