using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.Testing
{
    internal static class IStreamUnitOfWorkExtension
    {
        public static async Task<IStreamWriter> AddRangeAsync(this IStreamWriter unitOfWork, IEnumerable<EventRecord> records, CancellationToken token = default)
        {
            foreach (var record in records)
            {
                await unitOfWork.AddAsync(record.Id, record.Timestamp, record.Data!, token);
            }
            return unitOfWork;
        }

        public static async Task<IStreamWriter> AddRangeAsync(this Task<IStreamWriter> unitOfWork, IEnumerable<EventRecord> records, CancellationToken token = default)
        {
            await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await unitOfWork.Result.AddRangeAsync(records, token));
            return unitOfWork.Result;
        }

        public static async Task<IStreamWriter> SaveChangesAsync(this Task<IStreamWriter> unitOfWork, CancellationToken token = default)
        {
            await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await unitOfWork.Result.SaveChangesAsync(token));
            return unitOfWork.Result;
        }
    }
}
