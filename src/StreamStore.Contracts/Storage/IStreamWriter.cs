using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StreamStore
{
    public interface IStreamWriter
    {
        Task WriteAsync(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default);
    }
}
