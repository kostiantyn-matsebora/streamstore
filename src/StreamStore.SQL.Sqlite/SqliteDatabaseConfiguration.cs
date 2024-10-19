using System;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.SQL.Sqlite
{
    public class SqliteDatabaseConfiguration
    {
        public string ConnectionString { get; internal set; } = string.Empty;
        public string SchemaName { get; internal set; } = string.Empty;
        public string TableName { get; internal set; } = string.Empty;

        public string FullTableName => $"{SchemaName}.{TableName}";

        public bool ProvisionSchema { get; set; } = true;

        public bool EnableProfiling { get; set; } = false;

        internal SqliteDatabaseConfiguration()
        {
        }
    }

    public class SqliteDatabaseConfigurationBuilder : IConfigurator
    {
        string? connectionString;
        string name = "Events";
        string schema = "main";
        bool provisionSchema = true;
        bool enableProfiling = false;

        public SqliteDatabaseConfigurationBuilder WithConnectionString(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }

        public SqliteDatabaseConfigurationBuilder WithTable(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            return this;
        }

        public SqliteDatabaseConfigurationBuilder WithSchema(string schema)
        {
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
            return this;
        }

        public SqliteDatabaseConfigurationBuilder WithSchemaProvisioning(bool provisionSchema)
        {
            this.provisionSchema = provisionSchema;
            return this;
        }

        public SqliteDatabaseConfigurationBuilder WithProfiling()
        {
            this.enableProfiling = true;
            return this;
        }

        public SqliteDatabaseConfiguration Build()
        {
            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string must be set");
            }
            return new SqliteDatabaseConfiguration
            {
                ConnectionString = connectionString,
                SchemaName = schema,
                TableName = name,
                ProvisionSchema = provisionSchema,
                EnableProfiling = enableProfiling
            };
        }

        public virtual IServiceCollection Configure()
        {
            return new ServiceCollection();
        }
    }
}
