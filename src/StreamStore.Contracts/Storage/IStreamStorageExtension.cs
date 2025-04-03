using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static class IStreamStorageExtension
    {
        public static async Task<IStreamWriter> BeginAppendAsync(this IStreamStorage database, Id streamId, CancellationToken token = default)
        {
            return await database.BeginAppendAsync(streamId, Revision.Zero, token);
        }
    }
}
