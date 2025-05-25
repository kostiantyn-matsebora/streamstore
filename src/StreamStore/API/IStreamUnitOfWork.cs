using System.Threading.Tasks;
using System.Threading;



namespace StreamStore
{
    public interface IStreamUnitOfWork
    {
        Task<IStreamUnitOfWork> AppendAsync(IEventEnvelope envelope, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
