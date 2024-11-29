using System;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace StreamStore.Provisioning
{
    internal class SchemaProvisioningService : BackgroundService
    {
        readonly ISchemaProvisioner provisioner;

        public SchemaProvisioningService(ISchemaProvisioner provisioner)
        {
            this.provisioner = provisioner ?? throw new ArgumentNullException(nameof(provisioner));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await provisioner.ProvisionSchemaAsync(stoppingToken);
        }
    }
}
