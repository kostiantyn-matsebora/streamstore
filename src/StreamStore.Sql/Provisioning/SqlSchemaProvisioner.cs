using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.SQL;

namespace StreamStore.Sql
{
    internal sealed class SqlSchemaProvisioner
    {
        readonly SqlDatabaseConfiguration configuration;
        readonly IDbConnectionFactory connectionFactory;
        readonly IDapperCommandFactory commandFactory;

        public SqlSchemaProvisioner(SqlDatabaseConfiguration configuration, IDbConnectionFactory connectionFactory, IDapperCommandFactory commandFactory)
        {

            this.configuration = configuration.ThrowIfNull(nameof(configuration));
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.commandFactory = commandFactory.ThrowIfNull(nameof(commandFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                await connection.ExecuteAsync(commandFactory.CreateSchemaProvisioningCommand());
            }
        }
    }
}
