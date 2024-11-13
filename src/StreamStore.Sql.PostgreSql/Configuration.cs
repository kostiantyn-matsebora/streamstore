using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal static class Configuration
    {
        public const string ConfigurationSection = "StreamStore:PostgreSql";

        public readonly static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "public",
            TableName = "Events",
        };
    }
}
