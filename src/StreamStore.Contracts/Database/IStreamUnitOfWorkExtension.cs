using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public static class IStreamUnitOfWorkExtension
    {

        public static async Task<IStreamWriter> AddAsync(this Task<IStreamWriter> uow, Id eventId, DateTime timestamp, byte[] data, CancellationToken token = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await uow.Result.AddAsync(eventId, timestamp, data, CancellationToken.None));
        }

        public static async Task<IStreamWriter> SaveChangesAsync(this Task<IStreamWriter> unitOfWork, CancellationToken token = default)
        {
            await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await unitOfWork.Result.SaveChangesAsync(token));
            return unitOfWork.Result;
        }
    }
}
