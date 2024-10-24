using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static  class AsyncEnumerableExtension
    {
        public static async Task<EventEntityCollection> ReadToEndAsync(this IAsyncEnumerable<EventEntity> enumerable, CancellationToken cancellationToken = default)
        {
            var results = new List<EventEntity>();

            await foreach (var item in enumerable)
            {
                results.Add(item);
            }
            return new EventEntityCollection(results);
        }

        public static async Task<EventEntityCollection> ReadToEndAsync(this Task<IAsyncEnumerable<EventEntity>> enumerable, CancellationToken cancellationToken = default)
        {
            return await enumerable.Result.ReadToEndAsync(cancellationToken);
        }
    }
}
