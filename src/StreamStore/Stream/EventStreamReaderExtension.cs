
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static class EventStreamReaderExtension
    {
        public static async Task<EventEntityCollection> ReadToEndAsync(this Task<IEventStreamReader> reader, CancellationToken cancellationToken = default)
        {
            return await reader.Result.ReadToEndAsync(cancellationToken);
        }
    }
}
