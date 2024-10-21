using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;



namespace StreamStore
{
    partial class Stream
    {
        class ReadOnlyStream : IReadOnlyStream
        {
            private readonly ChannelReader<EventEntity> reader;

            public ReadOnlyStream(ChannelReader<EventEntity> reader)
            {
                this.reader = reader;
            }

            public IAsyncEnumerator<EventEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return reader.ReadAllAsync(cancellationToken).GetAsyncEnumerator();
            }

            public async Task<EventEntity[]> ReadToEnd(Revision startFrom, CancellationToken cancellationToken = default)
            {
                var results = new List<EventEntity>();
                await foreach (var item in reader.ReadAllAsync().WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    results.Add(item);
                }
                return results.ToArray();
            }
        }
    }
}
