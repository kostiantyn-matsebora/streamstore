using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;


namespace StreamStore
{
    public interface IEventStreamReader: IDisposable
    {
        IAsyncEnumerable<EventEntity> ReadAsync(CancellationToken cancellationToken = default);
        Task<EventEntityCollection> ReadToEndAsync(CancellationToken cancellationToken = default);
    }
}