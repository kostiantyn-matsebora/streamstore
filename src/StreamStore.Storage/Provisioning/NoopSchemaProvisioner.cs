using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Provisioning;

namespace StreamStore.Storage.Provisioning
{
    [ExcludeFromCodeCoverage]
    internal class NoopSchemaProvisioner : ISchemaProvisioner
    {
        public Task ProvisionSchemaAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}