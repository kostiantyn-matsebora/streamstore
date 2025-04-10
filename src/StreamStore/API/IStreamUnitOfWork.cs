using System;
using System.Threading.Tasks;
using System.Threading;



namespace StreamStore
{
    public interface IStreamUnitOfWork: IDisposable
    {
        Task<IStreamUnitOfWork> AppendAsync(IEventEnvelope envelope, CancellationToken cancellationToken = default);

        Task<Revision> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
