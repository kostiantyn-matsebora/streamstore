using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.Provisioning
{
    internal class DefaultSchemaProvisioner : ISchemaProvisioner
    {
        public Task ProvisionSchemaAsync(CancellationToken token)
        {
            // Do nothing
            return Task.CompletedTask;
        }
    }
}
