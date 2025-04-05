using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal static class SqliteConfiguration
    {
        public const string ConfigurationSection = "StreamStore:Sqlite";

        public readonly static SqlStorageConfiguration DefaultConfiguration = new SqlStorageConfiguration
        {
            SchemaName = "main",
            TableName = "Events",
        };
    }
}
