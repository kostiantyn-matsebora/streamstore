using System.Threading.Tasks;
using System.Threading;
using System;

namespace StreamStore
{
    public interface IStreamUnitOfWork: IDisposable
    {
        IStreamUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
