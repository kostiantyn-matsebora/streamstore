using System.Threading.Tasks;
using System.Threading;

namespace StreamStore.Sql.API
{
    public interface ISqlSchemaProvisioner
    {
        Task ProvisionSchemaAsync(CancellationToken token);
    }
}
