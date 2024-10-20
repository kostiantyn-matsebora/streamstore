using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


namespace StreamStore.SQL.Sqlite
{
    class SqliteSchemaProvisioningService : BackgroundService
    {
        readonly SqliteSchemaProvisioner provisioner;

        public SqliteSchemaProvisioningService(SqliteSchemaProvisioner provisioner)
        {
            this.provisioner = provisioner ?? throw new ArgumentNullException(nameof(provisioner));

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          await provisioner.ProvisionSchemaAsync(stoppingToken);
        }
    }
}
