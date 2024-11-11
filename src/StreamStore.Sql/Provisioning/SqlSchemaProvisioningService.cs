using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StreamStore.Sql.API;


namespace StreamStore.Sql.Provisioning
{
    class SqlSchemaProvisioningService : BackgroundService
    {
        readonly ISqlSchemaProvisioner provisioner;

        public SqlSchemaProvisioningService(ISqlSchemaProvisioner provisioner)
        {
            this.provisioner = provisioner ?? throw new ArgumentNullException(nameof(provisioner));

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await provisioner.ProvisionSchemaAsync(stoppingToken);
        }
    }
}
