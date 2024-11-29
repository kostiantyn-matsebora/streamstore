using System;


namespace StreamStore.Sql.Configuration
{
    public class SqlDatabaseConfiguration : ICloneable
    {
        public string ConnectionString { get; internal set; } = string.Empty;
        public string SchemaName { get; internal set; } = string.Empty;
        public string TableName { get; internal set; } = string.Empty;
        public string FullTableName => $"{SchemaName}.{TableName}";

        public SqlDatabaseConfiguration()
        {
        }

        public object Clone()
        {
            return new SqlDatabaseConfiguration
            {
                SchemaName = SchemaName,
                TableName = TableName,
                ConnectionString = ConnectionString
            };
        }
    }
}
