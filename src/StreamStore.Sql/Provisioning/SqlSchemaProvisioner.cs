using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Sql.API;

namespace StreamStore.Sql.Provisioning
{
    internal sealed class SqlSchemaProvisioner
    {
        readonly IDbConnectionFactory connectionFactory;
        readonly ISqlProvisioningQueryProvider queryProvider;

        public SqlSchemaProvisioner(IDbConnectionFactory connectionFactory, ISqlProvisioningQueryProvider queryProvider)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.queryProvider = queryProvider.ThrowIfNull(nameof(queryProvider));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                await connection.ExecuteAsync(queryProvider.GetSchemaProvisioningQuery());
            }
        }
    }
}
