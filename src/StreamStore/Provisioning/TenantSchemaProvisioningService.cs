using StreamStore.Multitenancy;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace StreamStore.Provisioning
{
    class TenantSchemaProvisioningService: BackgroundService
    {
        readonly ITenantProvider tenantProvider;
        readonly ITenantSchemaProvisionerFactory provisionerFactory;

        public TenantSchemaProvisioningService(ITenantSchemaProvisionerFactory provisionerFactory, ITenantProvider tenantProvider)
        {
            this.tenantProvider = tenantProvider.ThrowIfNull(nameof(tenantProvider));
            this.provisionerFactory = provisionerFactory.ThrowIfNull(nameof(provisionerFactory));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Parallel.ForEach(tenantProvider.GetAll(), async tenant =>
            {
                var provisioner = provisionerFactory.Create(tenant);
                await provisioner.ProvisionSchemaAsync(stoppingToken);
            });

            return Task.CompletedTask;
        }
    }
}
