using System.Threading;
using System.Threading.Tasks;
using StreamStore.Extensions;
using StreamStore.Provisioning;


namespace StreamStore.Sql.Provisioning
{
    internal sealed class SqlSchemaProvisioner: ISchemaProvisioner
    {
        private readonly IMigrator migrator;


        public SqlSchemaProvisioner(IMigrator migrator)
        {
            this.migrator = migrator.ThrowIfNull(nameof(migrator));
        }

        public Task ProvisionSchemaAsync(CancellationToken token)
        {
            migrator.Migrate();
            return Task.CompletedTask;
        }
    }
}
