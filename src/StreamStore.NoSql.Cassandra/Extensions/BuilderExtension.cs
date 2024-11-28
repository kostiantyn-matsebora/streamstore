using System;
using Cassandra;
using Microsoft.Extensions.Configuration;


namespace StreamStore.NoSql.Cassandra.Extensions {

 internal static class BuilderExtension
 {
    public static Builder UseAppConfig(this Builder builder, IConfiguration config, string connectionStringName = "StreamStore")
    {
        var connectionString = config.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");
        }

        builder.WithConnectionString(connectionString);
        return builder;
    }
 }
}
