using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public interface IStream
    {
        Task<IEventStreamWriter> BeginWriteAsync(Revision expectedRevision, CancellationToken cancellationToken = default);

        IEventStreamReader BeginRead(Revision startFrom, CancellationToken cancellationToken = default);
    }
}
