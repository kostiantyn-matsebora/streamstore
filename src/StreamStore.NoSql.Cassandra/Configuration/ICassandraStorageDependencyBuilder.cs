using System;
using System.Net.Security;
using Cassandra;
using Microsoft.Extensions.Configuration;


namespace StreamStore.NoSql.Cassandra.Configuration
{
    public interface ICassandraStorageDependencyBuilder
    {
        ICassandraStorageDependencyBuilder ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure);
        ICassandraStorageDependencyBuilder ConfigureCluster(Action<Builder> configure);
        ICassandraStorageDependencyBuilder UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null);
        ICassandraStorageDependencyBuilder UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null);
    }
}
