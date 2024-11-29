using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal static class SqliteConfiguration
    {
        public const string ConfigurationSection = "StreamStore:Sqlite";

        public readonly static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "main",
            TableName = "Events",
        };
    }
}
