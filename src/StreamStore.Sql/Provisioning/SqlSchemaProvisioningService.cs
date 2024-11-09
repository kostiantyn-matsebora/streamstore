using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


namespace StreamStore.Sql.Provisioning
{
    class SqlSchemaProvisioningService : BackgroundService
    {
        readonly SqlSchemaProvisioner provisioner;

        public SqlSchemaProvisioningService(SqlSchemaProvisioner provisioner)
        {
            this.provisioner = provisioner ?? throw new ArgumentNullException(nameof(provisioner));

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await provisioner.ProvisionSchemaAsync(stoppingToken);
        }
    }
}
