using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IEventStreamWriter> BeginWriteAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default);

        Task<IEventStreamReader> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default);

        Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default);
    }
}
