using System.Threading;
using System.Threading.Tasks;
using StreamStore.Provisioning;

namespace StreamStore.Storage.Provisioning
{
    internal class NoopSchemaProvisioner : ISchemaProvisioner
    {
        public Task ProvisionSchemaAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}