using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public static class IStreamWriterExtension
    {

        public static async Task<IStreamWriter> AppendAsync(this Task<IStreamWriter> uow, Id eventId, DateTime timestamp, byte[] data, CancellationToken token = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await uow.GetAwaiter().GetResult().AppendAsync(eventId, timestamp, data, CancellationToken.None));
        }

        public static async Task<IStreamWriter> CommitAsync(this Task<IStreamWriter> unitOfWork, CancellationToken token = default)
        {
            await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await unitOfWork.GetAwaiter().GetResult().ComitAsync(token));
            return unitOfWork.Result;
        }
    }
}
