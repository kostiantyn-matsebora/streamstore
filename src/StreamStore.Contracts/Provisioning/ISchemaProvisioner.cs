using System.Threading.Tasks;
using System.Threading;

namespace StreamStore.Provisioning
{
    public interface ISchemaProvisioner
    {
        Task ProvisionSchemaAsync(CancellationToken token);
    }
}
