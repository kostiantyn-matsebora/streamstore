using System;


namespace StreamStore.Sql.Configuration
{
    public class SqlStorageConfiguration : ICloneable
    {
        public string ConnectionString { get; internal set; } = string.Empty;
        public string SchemaName { get; internal set; } = string.Empty;
        public string TableName { get; internal set; } = string.Empty;
        public string FullTableName => $"{SchemaName}.{TableName}";

        public SqlStorageConfiguration()
        {
        }

        public object Clone()
        {
            return new SqlStorageConfiguration
            {
                SchemaName = SchemaName,
                TableName = TableName,
                ConnectionString = ConnectionString
            };
        }
    }
}
