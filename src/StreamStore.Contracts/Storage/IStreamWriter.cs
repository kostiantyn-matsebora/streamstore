using System.Threading.Tasks;
using System.Threading;
using System;

namespace StreamStore
{
    public interface IStreamWriter: IDisposable
    {
        Task<IStreamWriter> AppendAsync(Id eventId, DateTime timestamp, byte[] data, CancellationToken token = default);
        Task<Revision> ComitAsync(CancellationToken cancellationToken);
    }
}
