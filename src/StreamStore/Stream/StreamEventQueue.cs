using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;


namespace StreamStore
{

    internal class StreamEventQueue
    {
        readonly Channel<EventEntity> channel;

        public StreamEventQueue(int pageSize)
        {
            channel = Channel.CreateBounded<EventEntity>(new BoundedChannelOptions(pageSize)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = true
            });
        }
       
        public void Add(EventEntity entity)
        {
            channel.Writer.WriteAsync(entity).GetAwaiter().GetResult();
        }

        public IAsyncEnumerable<EventEntity> ReadAsync(CancellationToken cancellationToken = default)
        {
            return channel.Reader.ReadAllAsync(cancellationToken);
        }

        public void Complete()
        {
            channel.Writer.Complete();
        }
    }
}
