using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IReadOnlyStream : IAsyncEnumerable<EventEntity>
    {
        Task<EventEntity[]> ReadToEnd(Revision startFrom, CancellationToken cancellationToken = default);
    }
}