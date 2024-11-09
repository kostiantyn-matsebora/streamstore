using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Database;

namespace StreamStore
{
    public static class IStreamUnitOfWorkExtension
    {



        public static async Task<IStreamUnitOfWork> AddAsync(this Task<IStreamUnitOfWork> uow, Id eventId, DateTime timestamp, byte[] data, CancellationToken token = default)
        {
            await uow.Result.AddAsync(eventId, timestamp, data, CancellationToken.None);
            return uow.Result;
        }

        public static async Task<IStreamUnitOfWork> SaveChangesAsync(this Task<IStreamUnitOfWork> unitOfWork, CancellationToken token = default)
        {
            await unitOfWork.Result.SaveChangesAsync(token);
            return unitOfWork.Result;
        }
    }
}
