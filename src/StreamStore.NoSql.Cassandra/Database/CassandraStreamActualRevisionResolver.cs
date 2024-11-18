using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal static class CassandraStreamActualRevisionResolver
    {
        public static async Task<int> Resolve(DataContext ctx, Id streamId)
        {
            var id = (string)streamId;
            var revisions = (await ctx.StreamRevisions.Where(er => er.StreamId == id).Select(er => er.Revision).ExecuteAsync()).ToArray();
            if (!revisions.Any())
            {
                return Revision.Zero;
            }
            return revisions.Max();
        }
    }
}
