using System.Threading;
using System.Threading.Tasks;


namespace StreamDB
{
    public interface IStreamDB
    {
        Task AppendAsync(string streamId, IStreamItem[] uncommited, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(string streamId, CancellationToken ct = default);
        Task<IStream> GetAsync(string streamId, CancellationToken ct = default);
    }
}
