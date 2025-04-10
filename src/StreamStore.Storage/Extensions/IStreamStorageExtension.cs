using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static class IStreamStorageExtension
    {
        public static async Task<IStreamWriter> BeginAppendAsync(this IStreamStorage storage, Id streamId, CancellationToken token = default)
        {
            return await storage.BeginAppendAsync(streamId, Revision.Zero, token);
        }
    }
}
