using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using StreamStore.Models;

namespace StreamStore
{
    public static  class AsyncEnumerableExtension
    {
        public static async Task<StreamEventCollection> ReadToEndAsync(this IAsyncEnumerable<StreamEvent> enumerable, CancellationToken cancellationToken = default)
        {
            var results = new List<StreamEvent>();

            await foreach (var item in enumerable)
            {
                results.Add(item);
            }
            return new StreamEventCollection(results);
        }

        public static async Task<StreamEventCollection> ReadToEndAsync(this Task<IAsyncEnumerable<StreamEvent>> enumerable, CancellationToken cancellationToken = default)
        {
            return await enumerable.Result.ReadToEndAsync(cancellationToken);
        }
    }
}
