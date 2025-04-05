using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal static class PostgresConfiguration
    {
        public const string ConfigurationSection = "StreamStore:PostgreSql";

        public readonly static SqlStorageConfiguration DefaultConfiguration = new SqlStorageConfiguration
        {
            SchemaName = "public",
            TableName = "Events",
        };
    }
}
