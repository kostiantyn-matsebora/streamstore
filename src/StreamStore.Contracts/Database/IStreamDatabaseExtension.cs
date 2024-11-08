using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static class IStreamDatabaseExtension
    {
        public static async Task<IStreamUnitOfWork> BeginAppendAsync(this IStreamDatabase database, Id streamId, CancellationToken token = default)
        {
            return await database.BeginAppendAsync(streamId, Revision.Zero, token);
        }
    }
}
