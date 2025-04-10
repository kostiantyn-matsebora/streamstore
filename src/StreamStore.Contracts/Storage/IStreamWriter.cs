using System.Threading.Tasks;
using System.Threading;
using System;

namespace StreamStore
{
    public interface IStreamWriter: IDisposable
    {
        Task<IStreamWriter> AppendAsync(IEventRecord record, CancellationToken token = default);
        Task<Revision> ComitAsync(CancellationToken cancellationToken);
    }
}
